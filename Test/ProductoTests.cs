using Entity.Model;
using Xunit;

namespace Test
{
    public class ProductoTests
    {
        [Fact]
        public void CanCreateProducto_WithValidData()
        {
            var producto = new Producto
            {
                id_producto = 1,
                nombre = "Laptop",
                descripcion = "Portátil 16GB RAM",
                precio_unitario = 1500.50m,
                stock = 10,
                id_categoria = 2,
                estado = "Activo"
            };

            Assert.Equal(1, producto.id_producto);
            Assert.Equal("Laptop", producto.nombre);
            Assert.Equal("Portátil 16GB RAM", producto.descripcion);
            Assert.Equal(1500.50m, producto.precio_unitario);
            Assert.Equal(10, producto.stock);
            Assert.Equal(2, producto.id_categoria);
            Assert.Equal("Activo", producto.estado);
        }

        [Fact]
        public void Producto_DefaultValues_AreCorrect()
        {
            var producto = new Producto();
            Assert.Equal(0, producto.id_producto);
            Assert.Null(producto.nombre);
            Assert.Null(producto.descripcion);
            Assert.Equal(0, producto.precio_unitario);
            Assert.Equal(0, producto.stock);
            Assert.Equal(0, producto.id_categoria);
            Assert.Null(producto.estado);
        }
    }
}
