using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetCoreApiRest.InterfacesSQL;
using NetCoreApiRest.Models.ModelsSQL;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetCoreApiRest.Utils
{
	public static class ConfigurationSite
	{
		/// <summary>
		/// Variable para acceder al archivo appsettings.json para declarar las variables de configuracion
		/// que seran utilizadas dentro del WS
		/// Desarrollador: David Martinez
		/// </summary>
		public static IConfiguration _cofiguration;

		/// <summary>
		/// Variable para llenar dinamicamente los permisos desde la base de datos
		/// Desarrollador: David Martinez
		/// </summary>
		public static List<Permission> permissions;

		/// <summary>
		/// Metodo para inicializar la configuracion que es llamado desde la clase Startup.cs
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="cofiguration"></param>
		public static void WSConfig(IConfiguration cofiguration)
		{
			_cofiguration = cofiguration;
		}

		/// <summary>
		/// Metodo para traerse de forma dinamica los permisos establecidos en el sistema
		/// Desarrollador: David Martinez
		/// </summary>
		public static void GetPermission()
		{
			InicioSQL SQL = new InicioSQL();
			try
			{
				permissions = SQL.GetPermissionSystem();				
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para generar el token de sesion cuando se intente autenticar un usuario
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="claims">Clase con las variables necesarias incluyendo los permisos </param>
		/// <returns></returns>
		public static string GenerateToken(Claim[] claims)
		{
			var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cofiguration["ApiAuth:SecretKey"]));
			var credencial = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

			// Generamos el Token
			var token = new JwtSecurityToken
			(
				issuer: _cofiguration["ApiAuth:Issuer"],
				audience: _cofiguration["ApiAuth:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddHours(8),
				signingCredentials: credencial
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}

	/// <summary>
	/// Clase con el listado de permisos para los atributos que iran en el controlador
	/// Desarrollador: David Martinez
	/// </summary>
	public static class ConstantsPermission
	{
		public const string moduloPerfiles = "Módulo Perfiles";
		public const string agregarPerfiles = "Agregar Perfiles";
		public const string editarPerfiles = "Editar Perfiles";
		public const string eliminarPerfiles = "Eliminar Perfiles";

		public const string moduloUsuarios = "Módulo Usuarios";
		public const string agregarUsuarios = "Agregar Usuarios";
		public const string editarUsuarios = "Editar Usuarios";
		public const string eliminarUsuarios = "Eliminar Usuarios";

		public const string moduloCompañia = "Módulo Compañía";
		public const string agregarCompania = "Agregar Compañía";
		public const string editarCompañia = "Editar Compañía";
		public const string eliminarCompañia = "Eliminar Compañía";

		public const string moduloFamilia = "Módulo Familia";
		public const string agregarFamilia = "Agregar Familia";
		public const string editarFamilia = "Editar Familia";
		public const string eliminarFamilia = "Eliminar Familia";

		public const string visualizarEspecificacion = "Visualizar Especificación Técnica";
		public const string guardarDescripcion = "Guardar Descripción";
		public const string agregarEspecificacion = "Agregar Especificación Técnica";
		public const string editarEspecificacion = "Editar Especificación Técnica";
		public const string eliminarEspecificacion = "Eliminar Especificación Técnica";		

		public const string visualizarGarantiasServicio = "Visualizar Garantías y Servicios";
		public const string agregarGarantias = "Agregar Garantías";
		public const string eliminarGarantias = "Eliminar Garantías";
		public const string agregarPreguntas = "Agregar Preguntas Frecuentes";
		public const string editarPreguntas = "Editar Preguntas Frecuentes";
		public const string eliminarPreguntas = "Eliminar Preguntas Frecuentes";

		public const string visualizarProductosRelacionados = "Visualizar Productos Relacionados";
		public const string agregarProductosRelacionados = "Agregar Productos Relacionados";
		public const string editarProductosRelacionados = "Editar Productos Relacionados";
		public const string eliminarProductosRelacionados = "Eliminar Productos Relacionados";

		public const string moduloDirecciones = "Módulo Direcciones";
		public const string agregarDirecciones = "Agregar Direcciones";
		public const string editarDirecciones = "Editar Direcciones";
		public const string eliminarDirecciones = "Eliminar Direcciones";

		public const string moduloProductos = "Módulo Productos";
		public const string agregarProductos = "Agregar Productos";
		public const string editarProductos = "Editar Productos";
		public const string eliminarProductos = "Eliminar Productos";
		public const string importarProductos = "Importar Productos";
		public const string reGenerarProductos = "Re - generar etiquetas";

		public const string moduloEtiquetas = "Módulo Solicitud de Etiquetas";
		public const string agregarEtiquetas = "Agregar Solicitud";
		public const string agregarBitacora = "Agregar Bitácora";
		public const string visualizarHistorial = "Visualizar Historial de Seguimiento";

		public const string moduloVisor = "Módulo Visor";
		public const string agregarVisor = "Agregar Visor";
		public const string visualizarReporte = "Visualizar Reporte";

		public const string moduloConfiguracion = "Módulo Configuración";
		// Etiquetado y empacado
		public const string moduloEtiquetadoEmpacado = "Módulo Etiquetado y Empacado";
		public const string agregarModuloEmpacadoEtiquetado = "Agregar Empacado y Etiquetado";
		public const string editarModuloEmpacadoEtiquetado = "Editar Empacado y Etiquetado";
		public const string eliminarModuloEmpacadoEtiquetado = "Eliminar Empacado y Etiquetado";
		// Módulo de Estadisticas
		public const string moduloEstadisticas  = "Módulo de Estadísticas";
		public const string periodoTrimestal = "Periodo Trimestal";
		public const string periodoSemestral= "Periodo Semestral";
		public const string periodoAnual = "Periodo Anual";

		// Módulo de Direcciones de proveedor
		public const string consultaDireccionesProveedor = "Consultar Dirección de Proveedor";
		public const string agregarDireccionesProveedor = "Agregar Dirección de Proveedor";
		public const string editarDireccionesProveedor = "Editar Dirección de Proveedor";
		public const string eliminarDireccionesProveedor = "Eliminar Dirección de Proveedor";

		// Módulo de Configuracion de embalaje
		public const string consultaConfiguracionEmbalaje = "Consultar Configuración de Embajale";
		public const string editarConfiguracionEmbalaje = "Agregar Configuración de Embalaje";
		public const string eliminarConfiguracionEmbalaje = "Editar Configuración de Embalaje";
		// Submódulo Proveedores
		public const string agregarProveedor = "Agregar Proveedor";
		public const string editarProveedor = "Editar Proveedor";
		public const string eliminarProveedor = "Eliminar Proveedor";
		public const string asociarProductoProveedor = "Asociar Producto - Proveedor";
		// Submódulo Distribuidores
		public const string agregarDistribuidor = "Agregar Distribuidor";
		public const string editarDistribuidor = "Editar Distribuidor";
		public const string eliminarDistribuidor = "Eliminar Distribuidor";
		// Submódulo Empacado Externo
		public const string agregarEmpacadorExterno = "Agregar Empacador Externo";
		public const string editarEmpacadorExterno = "Editar Empacador Externo";
		public const string eliminarEmpacadorExterno = "Eliminar Empacador Externo";
		// Submódulo Empacado Interno
		public const string agregarEmpacadorInterno = "Agregar Empacador Interno";
		public const string editarEmpacadorInterno = "Editar Empacador Interno";
		public const string eliminarEmpacadorInterno = "Eliminar Empacador Interno";
		// Submódulo Empacado Interno y Externo
		public const string agregarOperador = "Agregar Operador";
		public const string editarOperador = "Editar Operador";
		public const string eliminarOperador = "Eliminar Operador";
		public const string agregarLinea = "Agregar Linea";
		public const string editarLinea = "Editar Linea";
		public const string eliminarLinea = "Eliminar Linea";
		// Submódulo Gestión de Cajas
		public const string asociarProducEmpaqEx = "Eliminar Linea";
	}

	/// <summary>
	/// Clase con el listado de roles para los atributos que iran en el controlador
	/// Desarrollador: David Martinez
	/// </summary>
	public static class Role
	{
		public const string Admin = "Admin";
		public const string User = "User";
	}
}
