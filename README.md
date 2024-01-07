# Swagger Globalization
Solution that implements multilanguage documentation and UI translation in .Net 8 swagger (Swashbuckle 6).

Based on configured supported cultures and default culture, you can:

1-enables accept-language header dropdown

2-globalize enums by resources based on display attribute

3-globalize swagger documentation

4-enables multidocumentation with different supported cultures by changing swagger "select definition" dropdown

5-enables UI translation (it is not native in the newest swagger 3.x version, so I've creared a workaround system that changes text when specific dom node is loaded; it is not an elegant way, but it works!)

Example includes english and italian supported cultures
