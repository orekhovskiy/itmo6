%{
#include <stdio.h>
#include <stdlib.h>
#include "tree.h"
%}


%union {
	struct ast *ast;
	int d;
	struct symbol *symbol;
	struct symbolList *symbolList;
}


%token <d> NUMBER
%token <symbol> SYMBOL
%token PLUS MINUS TIMES DIVISION GE LE COMA SEMICOLON ASSIGN LBRACKET RBRACKET EQUAL VAR BGN END IF UMINUS NOT REPEAT UNTIL


%nonassoc UMINUS NOT


%type <ast> variableDeclaration calculationDescription variablesList compoundOperator operatorList operator expression assign loopOperator
%type <symbolList> symbolList


%start program
%%


/* Программа */
program: 
	variableDeclaration calculationDescription { printAst($2, 0, NULL); }
	;

/* Объявление переменных */
variableDeclaration: 
	{ $$ = NULL; }
	| variablesList {$$ = $1; newAst('L', $1, NULL); }
	;


/* Описание вычислений */	
calculationDescription: 
	BGN operatorList END
	{ $$ = $2; }
	;


/* Список переменных */	
variablesList: 
	VAR symbolList SEMICOLON { $$ = newVariablesList($2); }
	;

symbolList: 
	SYMBOL	{ $$ = newSymbolList($1, NULL); }
	| SYMBOL COMA symbolList	  { $$ = newSymbolList($1, $3); }
	;
	
/* Список операторов */	
operatorList: 
	operator { $$ = $1; }
	| operator operatorList { $$ = newAst('L', $1, $2); }
	;

/* Оператор */	
operator: 
    assign SEMICOLON
	| compoundOperator
	| loopOperator
    ;
	
/* Составной оператор */	
compoundOperator: 
	BGN operatorList END
	{ $$ = $2; }
	;

/* Присваивание */
assign: 
	SYMBOL ASSIGN expression { $$ = newAssign($1, $3); }
	;
	
/* Выражение */
expression: 
	expression GE expression        { $$ = newAst('>', $1, $3); }
	| expression LE expression	    { $$ = newAst('<', $1, $3); }
	| expression EQUAL expression	{ $$ = newAst('=', $1, $3); }
	| expression PLUS expression	{ $$ = newAst('+', $1, $3); }
	| expression MINUS expression	{ $$ = newAst('-', $1, $3); }
	| expression TIMES expression	{ $$ = newAst('*', $1, $3); }
	| expression DIVISION expression { $$ = newAst('/', $1, $3); }
	| LBRACKET expression RBRACKET { $$ = $2; }
	| UMINUS expression %prec UMINUS { $$ = newAst('M', $2, NULL); }
	| NOT expression %prec NOT { $$ = newAst('M', $2, NULL); }
	| NUMBER { $$ = newNum($1); }
	| SYMBOL { $$ = newReference($1); }
	;


loopOperator:
	REPEAT operator UNTIL expression { $$ = newAst('U', $4, $2); }
	;
%%
