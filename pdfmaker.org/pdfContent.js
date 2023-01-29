module.exports={
    content: [
        {text: 'Tables', style: 'header'},
        {text: 'Headers', },
		'You can declare how many rows should be treated as a header. Headers are automatically repeated on the following pages',
		{text: ['It is also possible to set keepWithHeaderRows to make sure there will be no page-break between the header and these rows. Take a look at the document-definition and play with it. If you set it to one, the following table will automatically start on the next page, since there\'s not enough space for the first row to be rendered here']},
		{
			style: 'tableExample',
			table: {
				headerRows: 1,
				// dontBreakRows: true,
				// keepWithHeaderRows: 1,
				body: [
                    //encabezado de la tabla
					[{text: 'Header 1', style: 'tableHeader'},
                    {text: 'Header 2', style: 'tableHeader'},
                    {text: 'Header 3', style: 'tableHeader'}],
					[
                        //DATOS DE LA PRIMERA COLUMNA
						'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.',
						//SEGUNDA COLUMNA
                        'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.',
						//TERCERA COLUMNA
                        'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.',
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA SEGUNDA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA TERCERA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA SEGUNDA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA TERCERA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA SEGUNDA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA TERCERA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA SEGUNDA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA TERCERA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA SEGUNDA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA TERCERA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA SEGUNDA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA TERCERA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA SEGUNDA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA TERCERA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA TERCERA FILA DE LA TERCERA COLUMNA', 
					],
                    [
                        //DATOS DE LA PRIMERA COLUMNA
						'DATOS EN LA SEGUNDA FILA DE LA PRIMERA COLUMNA',
						//SEGUNDA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA SEGUNDA COLUMNA',
						//TERCERA COLUMNA
                       'DATOS EN LA SEGUNDA FILA DE LA TERCERA COLUMNA', 
					]
                    
				]
			}
            
		},
        {text: 'Defining row heights', style: 'subheader'},
		{
			style: 'tableExample',
			table: {
				heights: [],
				body: [
                    //TABLA SIN ENCABEZADO
					['row 1 with height 20', 'column B', 'column C'],
					['row 2 with height 50', 'column B', 'column C'],
					['row 3 with height 70', 'column B', 'column C'],
                    ['row 1 with height 20', 'column B', 'column C'],
					['row 2 with height 50', 'column B', 'column C'],
					['row 3 with height 70', 'column B', 'column C'],
                    ['row 1 with height 20', 'column B', 'column C'],
					['row 2 with height 50', 'column B', 'column C'],
					['row 3 with height 70', 'column B', 'column C'],
                    ['row 1 with height 20', 'column B', 'column C'],
					['row 2 with height 50', 'column B', 'column C'],
					['row 3 with height 70', 'column B', 'column C'],
                    ['row 1 with height 20', 'column B', 'column C'],
					['row 2 with height 50', 'column B', 'column C'],
					['row 3 with height 70', 'column B', 'column C'],
				]
			}
		}
    ]

}
   