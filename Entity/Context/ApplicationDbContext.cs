using Dapper;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;
using Module = Entity.Model.Module;


namespace Entity.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) 
        : base (options){ }

        public ApplicationDbContext() { }

        public DbSet<Module> Modules { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormModule> FormModules { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolFormPermission> RolFormPermissions { get; set; }
        public DbSet<Person> Persons { get; set; }

        // Nuevas entidades
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<ProductoProveedor> ProductoProveedores { get; set; }
        public DbSet<Bodega> Bodegas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<DetalleMovimiento> DetalleMovimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Restricciones de longitud para compatibilidad MySQL
            modelBuilder.Entity<Categoria>()
                .Property(c => c.nombre)
                .HasMaxLength(100);
            modelBuilder.Entity<Producto>()
                .Property(p => p.nombre)
                .HasMaxLength(100);
            modelBuilder.Entity<Producto>()
                .Property(p => p.estado)
                .HasMaxLength(20);
            modelBuilder.Entity<Proveedor>()
                .Property(pv => pv.nombre_empresa)
                .HasMaxLength(100);
            modelBuilder.Entity<Proveedor>()
                .Property(pv => pv.contacto)
                .HasMaxLength(100);
            modelBuilder.Entity<Proveedor>()
                .Property(pv => pv.telefono)
                .HasMaxLength(20);
            modelBuilder.Entity<Proveedor>()
                .Property(pv => pv.correo)
                .HasMaxLength(100);
            modelBuilder.Entity<Proveedor>()
                .Property(pv => pv.direccion)
                .HasMaxLength(150);
            modelBuilder.Entity<Bodega>()
                .Property(b => b.nombre)
                .HasMaxLength(100);
            modelBuilder.Entity<Bodega>()
                .Property(b => b.ubicacion)
                .HasMaxLength(150);
            modelBuilder.Entity<Bodega>()
                .Property(b => b.encargado)
                .HasMaxLength(100);
            modelBuilder.Entity<FormModule>()
                .HasOne(fm => fm.Form)
                .WithMany(f => f.FormModule)
                .HasForeignKey(fm => fm.form_id);

            modelBuilder.Entity<FormModule>()
                .HasOne(fm => fm.Module)
                .WithMany(m => m.FormModule)
                .HasForeignKey(fm => fm.module_id);

            modelBuilder.Entity<RolFormPermission>()
                .HasOne(rfp => rfp.Rol)
                .WithMany(r => r.RolFormPermission)
                .HasForeignKey(rfp => rfp.rol_id);

            modelBuilder.Entity<RolFormPermission>()
                .HasOne(rfp => rfp.Form)
                .WithMany(r => r.RolFormPermission)
                .HasForeignKey(rfp => rfp.form_id);

            modelBuilder.Entity<RolFormPermission>()
                .HasOne(rfp => rfp.Permission)
                .WithMany(r => r.RolFormPermission)
                .HasForeignKey(rfp => rfp.permission_id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Rol) //Un usuario tiene un Rol
                .WithMany(r => r.User) //Un Rol tiene muchos usuarios 
                .HasForeignKey(u => u.rol_id); //User LLeva la Foranea

            modelBuilder.Entity<Person>()
                .HasOne(p => p.User) //Una persona tiene un usuario
                .WithOne(u => u.Person) //Un usuario pertenece a una persona 
                .HasForeignKey<User>(u => u.person_id); //User lleva la Foranea

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // Relaciones para nuevas entidades
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.id_categoria);

            modelBuilder.Entity<ProductoProveedor>()
                .HasKey(pp => new { pp.id_producto, pp.id_proveedor });
            modelBuilder.Entity<ProductoProveedor>()
                .HasOne(pp => pp.Producto)
                .WithMany(p => p.ProductoProveedores)
                .HasForeignKey(pp => pp.id_producto);
            modelBuilder.Entity<ProductoProveedor>()
                .HasOne(pp => pp.Proveedor)
                .WithMany(pv => pv.ProductoProveedores)
                .HasForeignKey(pp => pp.id_proveedor);

            modelBuilder.Entity<DetalleMovimiento>()
                .HasKey(dm => new { dm.id_movimiento, dm.id_producto });
            modelBuilder.Entity<DetalleMovimiento>()
                .HasOne(dm => dm.Movimiento)
                .WithMany(m => m.DetalleMovimientos)
                .HasForeignKey(dm => dm.id_movimiento);
            modelBuilder.Entity<DetalleMovimiento>()
                .HasOne(dm => dm.Producto)
                .WithMany(p => p.DetalleMovimientos)
                .HasForeignKey(dm => dm.id_producto);

            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.Bodega)
                .WithMany(b => b.Movimientos)
                .HasForeignKey(m => m.id_bodega);
            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.id_usuario);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        public override int SaveChanges()
        {
            EnsureAudit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            EnsureAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void EnsureAudit()
        {
            ChangeTracker.DetectChanges();
        }

        // Métodos Dapper
        public async Task<IEnumerable<T>> QueryAsync<T>(string text, object? parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parameters ?? new { }, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryAsync<T>(command.Definition);
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string text, object? parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parameters ?? new { }, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(command.Definition);
        }


        /// <summary>
        /// Estructura para ejecutar comandos SQL con Dapper en Entity Framework Core.
        /// </summary>
        public readonly struct DapperEFCoreCommand : IDisposable
        {
            /// <summary>
            /// Constructor del comando Dapper.
            /// </summary>
            /// <param name="context">Contexto de la base de datos.</param>
            /// <param name="text">Consulta SQL.</param>
            /// <param name="parameters">Parámetros opcionales.</param>
            /// <param name="timeout">Tiempo de espera opcional.</param>
            /// <param name="type">Tipo de comando SQL opcional.</param>
            /// <param name="ct">Token de cancelación.</param>
            public DapperEFCoreCommand(DbContext context, string text, object parameters, int? timeout, CommandType? type, CancellationToken ct)
            {
                var transaction = context.Database.CurrentTransaction?.GetDbTransaction();
                var commandType = type ?? CommandType.Text;
                var commandTimeout = timeout ?? context.Database.GetCommandTimeout() ?? 30;

                Definition = new CommandDefinition(
                    text,
                    parameters,
                    transaction,
                    commandTimeout,
                    commandType,
                    cancellationToken: ct
                );
            }

            /// <summary>
            /// Define los parámetros del comando SQL.
            /// </summary>
            public CommandDefinition Definition { get; }

            /// <summary>
            /// Método para liberar los recursos.
            /// </summary>
            public void Dispose()
            {
            }
        }
    }
}
