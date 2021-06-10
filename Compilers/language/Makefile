all:
	bison -d grammar.y
	flex lexer.l
	cc -o tree tree.c grammar.tab.c lex.yy.c