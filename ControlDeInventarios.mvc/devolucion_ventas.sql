CREATE TABLE devolucion_ventas (
    PK_codigo INT IDENTITY(1,1) PRIMARY KEY,
    FK_clientes_ventas INT,
    fecha DATETIME,
    observaciones VARCHAR(MAX)
);

CREATE TABLE devolucion_venta_detalle (
    PK_codigo INT IDENTITY(1,1) PRIMARY KEY,
    FK_venta_detalle INT, 
    FK_devolucion_ventas INT,
);

CREATE VIEW vw_devolucion_ventas AS 
SELECT 
	dv.PK_codigo,
	dv.FK_clientes_pedidos,
	dv.fecha,
	cp.total
from devolucion_ventas AS dv
INNER JOIN clientes_pedidos AS cp ON cp.PK_codigo = dv.FK_clientes_pedidos;