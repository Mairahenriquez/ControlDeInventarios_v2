namespace ControlDeInventarios.mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            return;
            CreateTable(
                "dbo.bitacoras",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(),
                        FK_usuario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.bodegas",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        identificador = c.String(),
                        nombre = c.String(),
                        descripcion = c.String(),
                        observaciones = c.String(),
                        cantidad_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_usuario = c.Int(nullable: false),
                        FK_estado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.clientes",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        direccion = c.String(),
                        telefono = c.String(),
                        correo = c.String(),
                        nit = c.String(),
                        dui = c.String(),
                        nrc = c.String(),
                        fecha_nacimiento = c.DateTime(nullable: false),
                        giro = c.String(),
                        nombre_comercial = c.String(),
                        observaciones = c.String(),
                        imagen = c.String(),
                        abonos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        cargos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_municipio = c.Int(nullable: false),
                        FK_estado = c.Int(nullable: false),
                        FK_cuenta_contable = c.Int(nullable: false),
                        FK_usuario = c.Int(nullable: false),
                        FK_condicion_pago = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.clientes_pedidos",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        fecha = c.DateTime(nullable: false),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        observaciones = c.String(),
                        referencia = c.String(),
                        comentarios = c.String(),
                        FK_usuario = c.Int(nullable: false),
                        FK_condicion_pago = c.Int(nullable: false),
                        FK_estado = c.Int(nullable: false),
                        FK_cliente = c.Int(nullable: false),
                        FK_forma_pago = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.clientes_pedidos_detalle",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(),
                        cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_bodega = c.Int(nullable: false),
                        FK_inventario = c.Int(nullable: false),
                        FK_pedido = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.condiciones_pagos",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        dias = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.contabilidad_cuentas_contables",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        numero = c.String(),
                        nombre = c.String(),
                        cuenta_mayor = c.Boolean(nullable: false),
                        FK_cuenta_contable = c.Int(),
                        FK_clasificasion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.contabilidad_cuentas_contables_clasificaciones",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        identificador = c.String(),
                        nombre = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.contabilidad_partidas",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        concepto = c.String(),
                        fecha = c.DateTime(nullable: false),
                        fecha_hora = c.DateTime(nullable: false),
                        periodo = c.String(),
                        cargos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        abonos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        fecha_procesado = c.DateTime(),
                        FK_estado = c.Int(nullable: false),
                        FK_tipo_partida = c.Int(nullable: false),
                        FK_mes = c.Int(nullable: false),
                        FK_usuario = c.Int(nullable: false),
                        FK_compra = c.Int(),
                        FK_factura = c.Int(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.facturacion",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        numero = c.String(),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        fecha = c.DateTime(nullable: false),
                        abonado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        saldo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        concepto = c.String(),
                        fecha_procesada = c.DateTime(),
                        fecha_anulada = c.DateTime(),
                        fecha_finalizada = c.DateTime(),
                        observaciones = c.String(),
                        monto_retencion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        porcentaje_retencion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_cliente = c.Int(nullable: false),
                        FK_estado = c.Int(nullable: false),
                        FK_pedido = c.Int(nullable: false),
                        FK_movimiento = c.Int(),
                        FK_bodega = c.Int(nullable: false),
                        FK_usuario = c.Int(nullable: false),
                        FK_condicion_pago = c.Int(nullable: false),
                        FK_forma_pago = c.Int(nullable: false),
                        FK_partida = c.Int(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.formas_pagos",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.inventarios",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        identificador = c.String(),
                        descripcion = c.String(),
                        observaciones = c.String(),
                        imagen = c.String(),
                        fecha_vencimiento = c.DateTime(),
                        precio_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_cif = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_fob = c.Decimal(nullable: false, precision: 18, scale: 2),
                        comprable = c.Boolean(nullable: false),
                        vendible = c.Boolean(nullable: false),
                        precio_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        existencia_fisica = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_estado = c.Int(nullable: false),
                        FK_anio = c.Int(),
                        FK_estado_fisico = c.Int(nullable: false),
                        FK_cuenta_contable_inventarios = c.Int(nullable: false),
                        FK_cuenta_contable_costo_venta = c.Int(nullable: false),
                        FK_cuenta_contable_ingreso_venta = c.Int(nullable: false),
                        FK_cuenta_contable_devoluciones = c.Int(nullable: false),
                        porcentaje_ganacia = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ultimo_costo = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.inventarios_movimientos",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        fecha = c.DateTime(nullable: false),
                        fecha_hora = c.DateTime(nullable: false),
                        referencia = c.String(),
                        observaciones = c.String(),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_tipo_movimiento = c.Int(nullable: false),
                        FK_estado = c.Int(nullable: false),
                        FK_bodega = c.Int(nullable: false),
                        FK_usuario = c.Int(nullable: false),
                        FK_compra = c.Int(),
                        FK_factura = c.Int(),
                        FK_bodega_destino = c.Int(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.inventarios_movimientos_detalle",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(),
                        cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_bodega = c.Int(nullable: false),
                        FK_inventario = c.Int(nullable: false),
                        FK_movimiento = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.inventarios_movimientos_tipos",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.nota_debito_detalle",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        FK_nota_debito = c.Int(nullable: false),
                        concepto = c.String(),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.nota_debito",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        FK_facturacion = c.Int(nullable: false),
                        fecha_hora = c.DateTime(nullable: false),
                        numero = c.String(),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        observaciones = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.password_reset",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        FK_usuario = c.Int(nullable: false),
                        hash = c.String(),
                        vence = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.proveedores",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        razon_social = c.String(),
                        nombre_comercial = c.String(),
                        dui = c.String(),
                        nit = c.String(),
                        nrc = c.String(),
                        giro = c.String(),
                        telefono = c.String(),
                        direccion = c.String(),
                        fecha_hora = c.DateTime(nullable: false),
                        saldo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        abonado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_municipio = c.Int(nullable: false),
                        FK_tipo_contribuyente = c.Int(nullable: false),
                        FK_cuenta_contable = c.Int(nullable: false),
                        FK_estado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.proveedores_compras",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        fecha = c.DateTime(nullable: false),
                        documento = c.String(),
                        observaciones = c.String(),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        abono = c.Decimal(nullable: false, precision: 18, scale: 2),
                        saldo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_forma_pago = c.Int(nullable: false),
                        FK_orden_compra = c.Int(nullable: false),
                        FK_partida = c.Int(),
                        FK_estado = c.Int(nullable: false),
                        FK_usuario = c.Int(nullable: false),
                        FK_proveedor = c.Int(nullable: false),
                        FK_bodega = c.Int(nullable: false),
                        FK_movimiento = c.Int(),
                        FK_condicion_pago = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.proveedores_ordenes_compras",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(),
                        fecha = c.DateTime(nullable: false),
                        fecha_hora = c.DateTime(nullable: false),
                        observaciones = c.String(),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_bodega = c.Int(nullable: false),
                        FK_proveedor = c.Int(nullable: false),
                        FK_forma_pago = c.Int(nullable: false),
                        FK_partida = c.Int(),
                        FK_estado = c.Int(nullable: false),
                        FK_usuario = c.Int(nullable: false),
                        FK_condicion_pago = c.Int(nullable: false),
                        FK_movimiento = c.Int(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.proveedores_ordenes_compras_detalle",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(),
                        cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_orden_compra = c.Int(nullable: false),
                        FK_inventario = c.Int(nullable: false),
                        FK_bodega = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.usuarios",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        usuario = c.String(),
                        nombre = c.String(),
                        clave = c.String(),
                        telefono = c.String(),
                        correo = c.String(),
                        FK_estado = c.Int(nullable: false),
                        FK_rol = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.usuarios_roles",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_bodegas",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        identificador = c.String(),
                        nombre = c.String(),
                        descripcion = c.String(),
                        observaciones = c.String(),
                        cantidad_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_usuario = c.Int(nullable: false),
                        FK_estado = c.Int(nullable: false),
                        usuario = c.String(),
                        estado = c.String(),
                        estado_color = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_bodegas_detalle",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(),
                        cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_total = c.Decimal(precision: 18, scale: 2),
                        precio_total = c.Decimal(precision: 18, scale: 2),
                        existencias = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_bodega = c.Int(nullable: false),
                        FK_inventario = c.Int(nullable: false),
                        identificador = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_clientes",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        direccion = c.String(),
                        telefono = c.String(),
                        correo = c.String(),
                        nit = c.String(),
                        dui = c.String(),
                        nrc = c.String(),
                        fecha_nacimiento = c.DateTime(nullable: false),
                        giro = c.String(),
                        nombre_comercial = c.String(),
                        observaciones = c.String(),
                        imagen = c.String(),
                        abonos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        cargos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_municipio = c.Int(nullable: false),
                        FK_estado = c.Int(nullable: false),
                        FK_cuenta_contable = c.Int(nullable: false),
                        FK_usuario = c.Int(nullable: false),
                        FK_condicion_pago = c.Int(nullable: false),
                        municipio = c.String(),
                        departamento = c.String(),
                        estado = c.String(),
                        estado_color = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_clientes_pedidos",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        fecha = c.DateTime(nullable: false),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        observaciones = c.String(),
                        referencia = c.String(),
                        comentarios = c.String(),
                        FK_usuario = c.Int(nullable: false),
                        FK_condicion_pago = c.Int(nullable: false),
                        FK_estado = c.Int(nullable: false),
                        FK_cliente = c.Int(nullable: false),
                        FK_forma_pago = c.Int(nullable: false),
                        condicion_pago = c.String(),
                        estado = c.String(),
                        estado_color = c.String(),
                        cliente = c.String(),
                        forma_pago = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_clientes_pedidos_detalle",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(),
                        cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_bodega = c.Int(nullable: false),
                        FK_inventario = c.Int(nullable: false),
                        FK_pedido = c.Int(nullable: false),
                        inventario_identificador = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_contabilidad_cuentas_contables",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        numero = c.String(),
                        nombre = c.String(),
                        completo = c.String(),
                        cuenta_mayor = c.Boolean(nullable: false),
                        FK_cuenta_contable = c.Int(),
                        FK_clasificasion = c.Int(nullable: false),
                        cuenta_padre = c.String(),
                        clasificacion = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_contabilidad_partidas",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        concepto = c.String(),
                        fecha = c.DateTime(nullable: false),
                        fecha_hora = c.DateTime(nullable: false),
                        periodo = c.String(),
                        cargos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        abonos = c.Decimal(nullable: false, precision: 18, scale: 2),
                        fecha_procesado = c.DateTime(),
                        FK_estado = c.Int(nullable: false),
                        FK_tipo_partida = c.Int(nullable: false),
                        FK_mes = c.Int(nullable: false),
                        FK_usuario = c.Int(nullable: false),
                        FK_compra = c.Int(),
                        FK_factura = c.Int(),
                        estado = c.String(),
                        estado_color = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_contabilidad_partidas_detalle",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        numero_cuenta = c.String(),
                        nombre_cuenta = c.String(),
                        concepto = c.String(),
                        cargo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        abono = c.Decimal(nullable: false, precision: 18, scale: 2),
                        fecha = c.DateTime(nullable: false),
                        FK_partida = c.Int(nullable: false),
                        FK_cuenta_contable = c.Int(nullable: false),
                        FK_mes = c.Int(nullable: false),
                        FK_tipo = c.Int(nullable: false),
                        cuenta_contable = c.String(),
                        tipo = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_facturacion",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        fecha = c.DateTime(nullable: false),
                        abonado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        saldo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        concepto = c.String(),
                        fecha_procesada = c.DateTime(),
                        fecha_anulada = c.DateTime(),
                        fecha_finalizada = c.DateTime(),
                        observaciones = c.String(),
                        monto_retencion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        porcentaje_retencion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_cliente = c.Int(nullable: false),
                        FK_estado = c.Int(nullable: false),
                        FK_pedido = c.Int(nullable: false),
                        FK_movimiento = c.Int(),
                        FK_bodega = c.Int(nullable: false),
                        FK_usuario = c.Int(nullable: false),
                        FK_condicion_pago = c.Int(nullable: false),
                        FK_forma_pago = c.Int(nullable: false),
                        cliente = c.String(),
                        estado = c.String(),
                        estado_color = c.String(),
                        bodega = c.String(),
                        condicion_pago = c.String(),
                        forma_pago = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_facturacion_detalle",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(),
                        cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        descuento_aplicado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_bodega = c.Int(nullable: false),
                        FK_factura = c.Int(nullable: false),
                        FK_inventario = c.Int(nullable: false),
                        inventario_identificador = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_inventarios",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        identificador = c.String(),
                        descripcion = c.String(),
                        observaciones = c.String(),
                        imagen = c.String(),
                        fecha_vencimiento = c.DateTime(),
                        precio_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_cif = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_fob = c.Decimal(nullable: false, precision: 18, scale: 2),
                        comprable = c.Boolean(nullable: false),
                        vendible = c.Boolean(nullable: false),
                        precio_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        existencia_fisica = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_estado = c.Int(nullable: false),
                        FK_anio = c.Int(),
                        FK_estado_fisico = c.Int(nullable: false),
                        FK_cuenta_contable_inventarios = c.Int(nullable: false),
                        FK_cuenta_contable_costo_venta = c.Int(nullable: false),
                        FK_cuenta_contable_ingreso_venta = c.Int(nullable: false),
                        FK_cuenta_contable_devoluciones = c.Int(nullable: false),
                        cuenta_contable_inventarios = c.String(),
                        cuenta_contable_costo_venta = c.String(),
                        cuenta_contable_ingreso_venta = c.String(),
                        cuenta_contable_devoluciones = c.String(),
                        estado = c.String(),
                        estado_color = c.String(),
                        porcentaje_ganacia = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ultimo_costo = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_inventarios_existencias_por_bodegas",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        identificador = c.String(),
                        descripcion = c.String(),
                        existencia_fisica = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_inventario = c.Int(nullable: false),
                        FK_bodega = c.Int(nullable: false),
                        bodega = c.String(),
                        precio_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_total = c.Decimal(precision: 18, scale: 2),
                        precio_total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_inventarios_kardex_ventas",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        identificador = c.String(),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        estado = c.String(),
                        estado_color = c.String(),
                        descripcion = c.String(),
                        existencia_fisica = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_inventarios_movimientos",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        fecha = c.DateTime(nullable: false),
                        fecha_hora = c.DateTime(nullable: false),
                        referencia = c.String(),
                        observaciones = c.String(),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_tipo_movimiento = c.Int(nullable: false),
                        FK_estado = c.Int(nullable: false),
                        FK_bodega = c.Int(nullable: false),
                        FK_usuario = c.Int(nullable: false),
                        estado = c.String(),
                        estado_color = c.String(),
                        bodega = c.String(),
                        FK_compra = c.Int(),
                        FK_factura = c.Int(),
                        FK_bodega_destino = c.Int(),
                        bodega_destino = c.String(),
                        tipo_movimiento = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_inventarios_movimientos_detalle",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(),
                        cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        precio_total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_bodega = c.Int(nullable: false),
                        FK_inventario = c.Int(nullable: false),
                        FK_movimiento = c.Int(nullable: false),
                        identificador = c.String(),
                        producto = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_paises_municipios",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        FK_departamento = c.Int(nullable: false),
                        departamento = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_proveedores",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        razon_social = c.String(),
                        nombre_comercial = c.String(),
                        dui = c.String(),
                        nit = c.String(),
                        nrc = c.String(),
                        giro = c.String(),
                        telefono = c.String(),
                        direccion = c.String(),
                        fecha_hora = c.DateTime(nullable: false),
                        saldo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        abonado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_municipio = c.Int(nullable: false),
                        FK_tipo_contribuyente = c.Int(nullable: false),
                        FK_cuenta_contable = c.Int(nullable: false),
                        municipio = c.String(),
                        departamento = c.String(),
                        FK_estado = c.Int(nullable: false),
                        estado = c.String(),
                        estado_color = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_proveedores_compras",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        fecha = c.DateTime(nullable: false),
                        documento = c.String(),
                        observaciones = c.String(),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        abono = c.Decimal(nullable: false, precision: 18, scale: 2),
                        saldo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_forma_pago = c.Int(nullable: false),
                        FK_orden_compra = c.Int(nullable: false),
                        FK_partida = c.Int(),
                        FK_estado = c.Int(nullable: false),
                        FK_usuario = c.Int(nullable: false),
                        FK_proveedor = c.Int(nullable: false),
                        FK_bodega = c.Int(nullable: false),
                        FK_movimiento = c.Int(),
                        FK_condicion_pago = c.Int(nullable: false),
                        forma_pago = c.String(),
                        estado = c.String(),
                        estado_color = c.String(),
                        proveedor = c.String(),
                        bodega = c.String(),
                        condicion_pago = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_proveedores_compras_detalle",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(),
                        cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_compra = c.Int(nullable: false),
                        FK_inventario = c.Int(nullable: false),
                        FK_bodega = c.Int(nullable: false),
                        inventario_identificador = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_proveedores_ordenes_compras",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(),
                        fecha = c.DateTime(nullable: false),
                        fecha_hora = c.DateTime(nullable: false),
                        observaciones = c.String(),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_bodega = c.Int(nullable: false),
                        FK_proveedor = c.Int(nullable: false),
                        FK_forma_pago = c.Int(nullable: false),
                        FK_partida = c.Int(),
                        FK_estado = c.Int(nullable: false),
                        FK_usuario = c.Int(nullable: false),
                        FK_condicion_pago = c.Int(nullable: false),
                        FK_movimiento = c.Int(),
                        bodega = c.String(),
                        proveedor = c.String(),
                        forma_pago = c.String(),
                        partida = c.String(),
                        condicion_pago = c.String(),
                        estado = c.String(),
                        estado_color = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
            CreateTable(
                "dbo.vw_proveedores_ordenes_compras_detalle",
                c => new
                    {
                        PK_codigo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(),
                        cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        iva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FK_orden_compra = c.Int(nullable: false),
                        FK_inventario = c.Int(nullable: false),
                        FK_bodega = c.Int(nullable: false),
                        inventario_identificador = c.String(),
                    })
                .PrimaryKey(t => t.PK_codigo);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.vw_proveedores_ordenes_compras_detalle");
            DropTable("dbo.vw_proveedores_ordenes_compras");
            DropTable("dbo.vw_proveedores_compras_detalle");
            DropTable("dbo.vw_proveedores_compras");
            DropTable("dbo.vw_proveedores");
            DropTable("dbo.vw_paises_municipios");
            DropTable("dbo.vw_inventarios_movimientos_detalle");
            DropTable("dbo.vw_inventarios_movimientos");
            DropTable("dbo.vw_inventarios_kardex_ventas");
            DropTable("dbo.vw_inventarios_existencias_por_bodegas");
            DropTable("dbo.vw_inventarios");
            DropTable("dbo.vw_facturacion_detalle");
            DropTable("dbo.vw_facturacion");
            DropTable("dbo.vw_contabilidad_partidas_detalle");
            DropTable("dbo.vw_contabilidad_partidas");
            DropTable("dbo.vw_contabilidad_cuentas_contables");
            DropTable("dbo.vw_clientes_pedidos_detalle");
            DropTable("dbo.vw_clientes_pedidos");
            DropTable("dbo.vw_clientes");
            DropTable("dbo.vw_bodegas_detalle");
            DropTable("dbo.vw_bodegas");
            DropTable("dbo.usuarios_roles");
            DropTable("dbo.usuarios");
            DropTable("dbo.proveedores_ordenes_compras_detalle");
            DropTable("dbo.proveedores_ordenes_compras");
            DropTable("dbo.proveedores_compras");
            DropTable("dbo.proveedores");
            DropTable("dbo.password_reset");
            DropTable("dbo.nota_debito");
            DropTable("dbo.nota_debito_detalle");
            DropTable("dbo.inventarios_movimientos_tipos");
            DropTable("dbo.inventarios_movimientos_detalle");
            DropTable("dbo.inventarios_movimientos");
            DropTable("dbo.inventarios");
            DropTable("dbo.formas_pagos");
            DropTable("dbo.facturacion");
            DropTable("dbo.contabilidad_partidas");
            DropTable("dbo.contabilidad_cuentas_contables_clasificaciones");
            DropTable("dbo.contabilidad_cuentas_contables");
            DropTable("dbo.condiciones_pagos");
            DropTable("dbo.clientes_pedidos_detalle");
            DropTable("dbo.clientes_pedidos");
            DropTable("dbo.clientes");
            DropTable("dbo.bodegas");
            DropTable("dbo.bitacoras");
        }
    }
}
