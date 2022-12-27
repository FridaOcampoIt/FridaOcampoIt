export const enviroments = {
  /*  urlBase: "https://www.traceit.net:3100/",
    qrUrl: "https://data.traceit.net/tracking?qr=",*/
    urlBase: "http://localhost:5101/",//localhost
    qrUrl: "http://localhost:5101/tracking?qr=",//localhodt
    
    pageSize: [20, 50, 100],

    patterns: {
        email: "^[a-zA-Z0-9._$%&+-]+@[a-zA-Z0-9._$%&+-]+\\.[a-zA-Z0-9._$%&+-]+$",
        emailInn: '[a-zA-Z0-9._+@-]',
        poInn: '[a-zA-Z0-9_#\\/-]',
        alphabetic: '[a-zA-ZñÑ ]+',
        alphanumeric: '[a-zA-ZñÑ0-9 ]+',
        alphanumerics: '^[+-]?(?:[a-zA-ZÑñ0-9])[A-Za-zÑñ\\d]*$',
        alphabeticSpecial: '[a-zA-ZñÑ@._$%& -]+',
        alphanumericSpecial: '[0-9a-zA-ZñÑ@._$%& -]+',
        numerical: '^[0-9+.]+$',
        decimal: '^[+-]?[0-9+]+(\.[0-9]{1,10}){0,1}$'
    }
}