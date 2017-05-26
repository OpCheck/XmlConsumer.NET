grammar XmlConsumer;

xmlConsumerExpressionFile : xmlConsumerScript EOF
	;

xmlConsumerScript : optionsTuple? objectMappingBlock
	;

optionsTuple : '(' optionsList ')'
	;

optionsList : option (';' option)* ';'?
	|
	;

option : 'ExpressionType' ':' STRING_LITERAL # ExpressionTypeOption
	| 'Encoding' ':' STRING_LITERAL # EncodingOption
	| 'Prefix' ':' STRING_LITERAL ',' STRING_LITERAL # PrefixOption
	| 'Binding' ':' STRING_LITERAL # BindingOption
	| 'Bindings' ':' stringLiteralsList # BindingsOption
	| 'ParseMethod' ':' STRING_LITERAL # ParseMethodOption
	| 'AutoParseMode' ':' STRING_LITERAL # AutoParseModeOption
	| 'AutoCreateMode' ':' STRING_LITERAL # AutoCreateModeOption
	| 'NullObjectTreatmentMode' ':' STRING_LITERAL # NullObjectTreatmentModeOption
	| 'Collectivity' ':' STRING_LITERAL # CollectivityOption
	;
	
stringLiteralsList : STRING_LITERAL (',' STRING_LITERAL)*
	|
	;
	
objectMappingBlock : '{' mappingList '}'
	;

mappingList : ((simpleMapping ';') | complexMapping)*
	;

simpleMapping : OBJECT_MEMBER_IDENTIFIER ':' 'null' # NullMapping
	| OBJECT_MEMBER_IDENTIFIER optionsTuple ':' 'null' # NullMappingWithOptions
	| OBJECT_MEMBER_IDENTIFIER ':' STRING_LITERAL # StringMapping
	| OBJECT_MEMBER_IDENTIFIER optionsTuple ':' STRING_LITERAL # StringMappingWithOptions
	;

complexMapping : OBJECT_MEMBER_IDENTIFIER ':' objectMappingBlock # ObjectMapping
	| OBJECT_MEMBER_IDENTIFIER optionsTuple ':' objectMappingBlock # ObjectMappingWithOptions
	;

STRING_LITERAL : '\'' ('\\\\' | '\\\'' | .)*? '\'';
OBJECT_MEMBER_IDENTIFIER : [a-zA-Z_][a-zA-Z_0-9]*;
WHITESPACE : [ \t\r\n]+ -> skip;
LINE_COMMENT : '//' .*? '\r'? '\n' -> skip;
