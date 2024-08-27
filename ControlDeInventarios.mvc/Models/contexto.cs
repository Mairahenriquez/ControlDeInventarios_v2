using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ControlDeInventarios.entities;

namespace ControlDeInventarios.mvc.Models
{
    public class contexto : DbContext
    {
        public contexto() : base("Conexion")
        {

        }
        public DbSet<bitacoras> bitacoras { get; set; }
        public DbSet<contabilidad_cuentas_contables> contabilidad_cuentas_contables { get; set; }
        public DbSet<vw_contabilidad_cuentas_contables> vw_contabilidad_cuentas_contables { get; set; }
        public DbSet<vw_paises_municipios> vw_paises_municipios { get; set; }
        public DbSet<contabilidad_cuentas_contables_clasificaciones> contabilidad_cuentas_contables_clasificaciones { get; set; }
        public DbSet<bodegas> bodegas { get; set; }
        public DbSet<vw_bodegas> vw_bodegas { get; set; }
        public DbSet<vw_bodegas_detalle> vw_bodegas_detalle { get; set; }
        public DbSet<vw_inventarios_existencias_por_bodegas> vw_inventarios_existencias_por_bodegas { get; set; }
        public DbSet<proveedores> proveedores { get; set; }
        public DbSet<vw_proveedores> vw_proveedores { get; set; }
        public DbSet<proveedores_compras> proveedores_compras { get; set; }
        public DbSet<vw_proveedores_compras> vw_proveedores_compras { get; set; }
        public DbSet<vw_proveedores_compras_detalle> vw_proveedores_compras_detalle { get; set; }
        public DbSet<proveedores_ordenes_compras> proveedores_ordenes_compras { get; set; }
        public DbSet<vw_proveedores_ordenes_compras> vw_proveedores_ordenes_compras { get; set; }
        public DbSet<proveedores_ordenes_compras_detalle> proveedores_ordenes_compras_detalle { get; set; }
        public DbSet<vw_proveedores_ordenes_compras_detalle> vw_proveedores_ordenes_compras_detalle { get; set; }
        public DbSet<inventarios_movimientos> inventarios_movimientos { get; set; }
        public DbSet<vw_inventarios_movimientos> vw_inventarios_movimientos { get; set; }
        public DbSet<inventarios_movimientos_detalle> inventarios_movimientos_detalle { get; set; }
        public DbSet<vw_inventarios_movimientos_detalle> vw_inventarios_movimientos_detalle { get; set; }
        public DbSet<inventarios_movimientos_tipos> inventarios_movimientos_tipos { get; set; }
        public DbSet<vw_contabilidad_partidas> vw_contabilidad_partidas { get; set; }
        public DbSet<formas_pagos> formas_pagos { get; set; }
        public DbSet<condiciones_pagos> condiciones_pagos { get; set; }
        public DbSet<vw_inventarios> vw_inventarios { get; set; }
        public DbSet<clientes> clientes { get; set; }
        public DbSet<vw_clientes> vw_clientes { get; set; }
        public DbSet<clientes_pedidos> clientes_pedidos { get; set; }
        public DbSet<vw_clientes_pedidos> vw_clientes_pedidos { get; set; }
        public DbSet<clientes_pedidos_detalle> clientes_pedidos_detalle { get; set; }
        public DbSet<vw_clientes_pedidos_detalle> vw_clientes_pedidos_detalle { get; set; }
        public DbSet<facturacion> facturacion { get; set; }
        public DbSet<vw_facturacion> vw_facturacion { get; set; }
        public DbSet<vw_facturacion_detalle> vw_facturacion_detalle { get; set; }
        public DbSet<inventarios> inventarios { get; set; }
        public DbSet<vw_contabilidad_partidas_detalle> vw_contabilidad_partidas_detalle { get; set; }
        public DbSet<contabilidad_partidas> contabilidad_partidas { get; set; }
        public DbSet<usuarios> usuarios { get; set; }
        public DbSet<usuarios_roles> usuarios_roles { get; set; }
        public DbSet<password_reset> password_reset { get; set; }
        public DbSet<vw_inventarios_kardex_ventas> vw_inventarios_kardex_ventas { get; set; }
    }
}