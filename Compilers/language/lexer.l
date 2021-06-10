%option noyywrap yylineno 
%{
#include "tree.h"
#include "grammar.tab.h"
%}

%%
"-"     { return MINUS; }
"+"	    { return PLUS; }
"*"	    { return TIMES; }
"/"	    { return DIVISION; }
">="     { return GE; }
"=<"     { return LE; }
"="     { return EQUAL; }
"not"     { return NOT; }

"("     { return LBRACKET; }
")"     { return RBRACKET; }

","	    { return COMA; }
";"	    { return SEMICOLON; }
":=" 	{ return ASSIGN; }

"Var"   { return VAR; }
"Begin" { return BGN; }
"End"   { return END; }

"REPEAT" { return REPEAT; }
"UNTIL"  { return UNTIL; }

[a-z]+	{ yylval.symbol = lookUp(yytext); return SYMBOL; }
[0-9]+	{ yylval.d = atoi(yytext); return NUMBER; }

"/*"(.|\n)*?"*/" 

[ \t\r\n]
.	{ yyerror("Mystery character %c\n", *yytext); }
%%