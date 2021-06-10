#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdarg.h>
#include <math.h>
#include "tree.h"

static unsigned symHash(char *symbol) {
	unsigned int hash = 0;

	unsigned ch;

	while(ch = *symbol++) {
		hash = hash * 9 ^ ch;

		return hash;
	}
}

struct symbol *lookUp(char *symbol) {
	struct symbol *s = &symtab[symHash(symbol) % NHASH];
	int scount = NHASH;

	while(--scount >= 0) {
		if(s->name && !strcmp(s->name, symbol)) {
			return s;
		}
		if(!s->name) {
			s->name = strdup(symbol);
			s->value = 0;
			return s;
		}
		if(++s >= symtab + NHASH) {
			s = symtab;
		}
	}

	yyerror("symbol table overflow\n");
	abort();
}

struct ast *newAst(int nodeType, struct ast *left, struct ast *right) {
	struct ast *a = malloc(sizeof(struct ast));

	if(!a) {
		yyerror("out of space");
		exit(0);
	}

	a->nodeType = nodeType;
	a->left = left;
	a->right = right;

	return a;
}

struct ast *newNum(double number) {
	struct numVal *n = malloc(sizeof(struct numVal));

	if(!n) {
		yyerror("out of space");
		exit(0);
	}

	n->nodeType = 'K';
	n->number = number;
	return (struct ast *)n;
}

struct ast *newCmp(int compareType, struct ast *left, struct ast *right) {
	struct ast *a = malloc(sizeof(struct ast));

	if(!a) {
		yyerror("out of space");
		exit(0);
	}

	a->nodeType = '0' + compareType;
	a->left = left;
	a->right = right;
	return a;
}

struct ast *newReference(struct symbol *symbol) {
	struct symbolReference *sr = malloc(sizeof(struct symbolReference));

	if(!sr) {
		yyerror("out of space");

		exit(0);
	}

	sr->nodeType = 'R';
	sr->symbol = symbol;

	return (struct ast *)sr;
}

struct ast *newAssign(struct symbol *symbol, struct ast *v) {
	struct symbolAssign *sa = malloc(sizeof(struct symbolAssign));

	if(!sa) {
		yyerror("out of space");
		exit(0);
	}

	sa->nodeType = 'A';
	sa->symbol = symbol;
	sa->v = v;

	return (struct ast *)sa;
}


struct ast *newVariablesList(struct symbolList *symbolList) {
	struct declaration *d = malloc(sizeof(struct declaration));

	if(!d) {
		yyerror("out of space");
		
		exit(0);
	}

	d->nodeType ='X';
	d->symbolList = symbolList;
	return (struct ast *)d;
}


struct symbolList *newSymbolList(struct symbol *symbol, struct symbolList *next) {
	struct symbolList *sl = malloc(sizeof(struct symbolList));

    	if(!sl) {
        	yyerror("out of space");

        	exit(0);
 	}
 
    	sl->symbol = symbol;
    	sl->next = next;
     
	return sl;
}

struct numList *newNumList(double number, struct numList *next) {
    struct numList *nl = malloc(sizeof(struct numList));
    
    if(!nl) {
        yyerror("out of space");
        
        exit(0);
    }   
    
    nl->number = number;
    nl->next = next;

    return nl;
}

void drawOffset(int offset)
{
	printf("\033[%dC", offset);
}

void drawLine(int offset, int height)
{
	printf("\033[%dA", height); // move cursor up by height lines
	printf("\033[%dC", offset);

	for (int i = 0; i < height; i++) {
		printf("|");
		printf("\033[1B\033[1D");
	}
	printf("\r");
}

int drawNode(int parentOffset, const char * nodeName, const char * nodeDescription, const char * branchDescription)
{
	char branchStrBuffer[255];

	drawOffset(parentOffset);
	printf("|\n");

	drawOffset(parentOffset);
	if (branchDescription) {
		sprintf(branchStrBuffer, "|[%s]>(%s) {%s}\n", branchDescription, nodeName, nodeDescription);
	} else {
		sprintf(branchStrBuffer, "|>(%s) {%s}\n", nodeName, nodeDescription);
	}

	printf("%s", branchStrBuffer);

	return parentOffset + strlen(branchStrBuffer) - strlen(nodeName)/2 - strlen(nodeDescription) - 4;
}

int printAst(struct ast * a, int offset, const char * description)
{
	if (a == NULL) {
		return 0;
	}

	if (offset == 0) {
		printf("root\n");
		offset = 2;
	}

	int branchesHeight = 0;

	switch(a->nodeType) {
		case '+':
		case '-':
		case '*':
		case '/': 
		{
			char buf[10];
			sprintf(buf, "%c", a->nodeType);
			int childrenOffset = drawNode(offset, buf, "binary operator", description);
			branchesHeight += printAst(a->right, childrenOffset, NULL);
			drawLine(childrenOffset, branchesHeight);
			branchesHeight += printAst(a->left, childrenOffset, NULL);
			branchesHeight += 2;
			break;
		}
		case '=': 
		{
			int childrenOffset = drawNode(offset, "=", "compare operator", description);
			branchesHeight += printAst(a->right, childrenOffset, NULL);
			branchesHeight += printAst(a->left, childrenOffset, NULL);
			drawLine(childrenOffset, branchesHeight);
			branchesHeight += 2;
			break;
		}
		case '>':
		{
			int childrenOffset = drawNode(offset, ">=", "compare operator", description);
			branchesHeight += printAst(a->right, childrenOffset, NULL);
			branchesHeight += printAst(a->left, childrenOffset, NULL);
			drawLine(childrenOffset, branchesHeight);
			branchesHeight += 2;
			break;
		}
		case '<': 
		{
			int childrenOffset = drawNode(offset, "=<", "compare operator", description);
			branchesHeight += printAst(a->right, childrenOffset, NULL);
			branchesHeight += printAst(a->left, childrenOffset, NULL);
			drawLine(childrenOffset, branchesHeight);
			branchesHeight += 2;
			break;
		}
		case 'U': 
		{
			int childrenOffset = drawNode(offset, "loop", "Branching", description);

			branchesHeight += printAst( a->left, childrenOffset, "expr");
			drawLine(childrenOffset, branchesHeight);
			branchesHeight += printAst( a->right, childrenOffset, NULL);

			branchesHeight += 2;
			break;
		}
		case 'L':
		{
			branchesHeight += printAst(a->right, offset, NULL);
			drawLine(offset, branchesHeight);
			branchesHeight += printAst(a->left, offset, NULL);
			break;
		}
		case 'R':
		{
			char * name = ( (struct symbolReference *)a)->symbol->name;
			drawNode(offset, name, "identifier", description);
			branchesHeight += 2; 
			break;
		}
		case 'M': 
		{
			int childrenOffset = drawNode(offset, "not", "unary operator", description);
			branchesHeight += printAst(a->left, childrenOffset, NULL);
			branchesHeight += 2; 
			break;
		}
		case 'K':
		{
			char buf[10];
			sprintf(buf, "%d", ( (struct numVal *)a)->number);
			drawNode(offset, buf, "const", description);
			branchesHeight += 2; 
			break;
		}
		case 'A':
		{
			int childrenOffset = drawNode(offset, ":=", "assign", description);
			
			branchesHeight += printAst(( (struct symbolAssign *)a)->v, childrenOffset, NULL);

			char * name = ( (struct symbolAssign *)a)->symbol->name;
			drawNode(childrenOffset, name, "identifier", NULL);

			branchesHeight += 2;
			drawLine(childrenOffset, branchesHeight);

			branchesHeight += 2; 
			break;
		}
		case 'X':

			break;

		default: printf("internal error: bad node %c\n", a->nodeType);
	}

	return branchesHeight;
}

void treeFree(struct ast *a) {
	switch(a->nodeType) {
		case '+':
		case '-':
		case '*':
		case '/':
		case '>': 
		case '=': 
		case 'I':	
		case 'L':
			treeFree(a->right);
			treeFree(a->left);
		case 'R':
		case 'M': 
			treeFree(a->left);
		case 'K': 
		case 'A':
			free(((struct symbolAssign *)a)->v);

			break;
		case 'W':
			free(a->left);

			if(a->right) {
				treeFree(a->right);
			}

			break;
		case 'X':
			free(((struct declaration *)a)->symbolList);

			break;

		default: printf("internal error: free bad node %c\n", a->nodeType);
	}

	free(a);
}


static double callDeclaration(struct declaration *);


static double callDeclaration(struct declaration *d) {
	while(d->symbolList) {
		((d->symbolList)->symbol)->value = 0;
		((d->symbolList)->symbol)->type = 'a';
		((d->symbolList)->symbol)->arrLength = 0;
		((d->symbolList)->symbol)->initialIndex = 0;
		((d->symbolList)->symbol)->arrHead = NULL;

		d->symbolList = (d->symbolList)->next;
	}

	return 0;
}

void yyerror(char *s, ...)
{
  va_list ap;
  va_start(ap, s);

  fprintf(stderr, "%d: error: ", yylineno);
  vfprintf(stderr, s, ap);
  fprintf(stderr, "\n");
}

int main(int argc, char**argv) {
	extern FILE *yyin;

	++argv; 
	--argc;

	yyin = fopen(argv[0], "r");

	return yyparse();
}