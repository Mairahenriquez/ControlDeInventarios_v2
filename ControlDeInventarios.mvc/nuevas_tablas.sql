CREATE TABLE devolucion_compras (
    PK_codigo INT  IDENTITY(1,1)  PRIMARY KEY,
    FK_proveedores_compras INT,
    fecha DATETIME,
    observaciones VARCHAR(MAX)
);
CREATE TABLE devolucion_compra_detalle (
    PK_codigo INT  IDENTITY(1,1) PRIMARY KEY,
    FK_compra_detalle INT , 
    FK_devolucion_compra INT
);