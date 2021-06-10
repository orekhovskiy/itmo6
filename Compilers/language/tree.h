extern int yylineno;

void yyerror (char *s, ...);
int yyparse();
int yylex();

struct symbol 
{
	double *arrHead;
	double value;

	char *name;
	char type;

	int arrLength;
	int initialIndex;
};

#define NHASH 9997
struct symbol symtab[NHASH];

struct symbol *lookUp(char *);

struct symbolList 
{
	struct symbol *symbol;
	struct symbolList *next;
};

struct numList 
{
	double number;
	struct numList *next;
};

struct symbolList *newSymbolList(struct symbol *symbol, struct symbolList *next);

struct ast 
{
	int nodeType;

	struct ast *left;
	struct ast *right;
};


struct numVal 
{
	int nodeType;
	int number;
};

struct symbolReference
{
	int nodeType;
	struct symbol *symbol;
};

struct symbolAssign 
{
	int nodeType;
	struct symbol *symbol;
	struct ast *v;
};

struct declaration 
{
	int nodeType;
	struct symbolList *symbolList;
};

struct ast *newAst(int nodeType, struct ast *left, struct ast *right);
struct ast *newCompare(int compareType, struct ast *left, struct ast *right);
struct ast *newReference(struct symbol *symbol);
struct ast *newAssign(struct symbol *symbol, struct ast *v);
struct ast *newNum(double number);
struct ast *newFlow(int nodeType, struct ast *cond, struct ast *tl);
struct ast *newVariablesList(struct symbolList *symbolList);

int printAst(struct ast * a, int level, const char * description);

void treeFree(struct ast *);
