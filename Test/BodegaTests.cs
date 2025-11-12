using Entity.Model;
using Xunit;

namespace Test
{
    public class BodegaTests
    {
        [Fact]
        public void CanCreateBodega_WithValidData()
        {
            var bodega = new Bodega
            {
                id_bodega = 1,
                nombre = "Central",
                ubicacion = "Calle 123",
                capacidad_maxima = 1000,
                encargado = "Juan Perez"
            };

            Assert.Equal(1, bodega.id_bodega);
            Assert.Equal("Central", bodega.nombre);
            Assert.Equal("Calle 123", bodega.ubicacion);
            Assert.Equal(1000, bodega.capacidad_maxima);
            Assert.Equal("Juan Perez", bodega.encargado);
        }

        [Fact]
        public void Bodega_DefaultValues_AreCorrect()
        {
            var bodega = new Bodega();
            Assert.Equal(0, bodega.id_bodega);
            Assert.Null(bodega.nombre);
            Assert.Null(bodega.ubicacion);
            Assert.Equal(0, bodega.capacidad_maxima);
            Assert.Null(bodega.encargado);
        }
    }
}
