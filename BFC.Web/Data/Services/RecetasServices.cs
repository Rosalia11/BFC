using BFC.Web.Data.Dtos;
using BFC.Web.Data.Entities;
using BFC.Web.Migrations;
using Microsoft.EntityFrameworkCore;

namespace BFC.Web.Data.Services
{
    public interface IRecetasServices
    {
        Task<RecestasDtos?> GetByIdAsync(int id);
        Task<IReadOnlyList<RecestasDtos>> GetByCategoryAsync(string categoria);
        Task<IEnumerable<RecestasDtos>> GetAllAsync();
    }

    public class RecetasServices : IRecetasServices
    {
        private readonly ApplicationDbContext _ctx;
        public RecetasServices(ApplicationDbContext ctx) => _ctx = ctx;
        public async Task<RecestasDtos?> GetByIdAsync(int id) =>
            await _ctx.Recetas
                      .AsNoTracking()
                      .Where(r => r.Id_Recetas == id)          // la entidad usa Id
                      .Select(r => new RecestasDtos
                      {
                          Id_Recetas = r.Id_Recetas,    // mapeas al DTO
                          Nombre = r.Nombre,
                          Categoria = r.Categoria,
                          Tiempo_preparacion = r.Tiempo_preparacion,
                          Imagen = r.Imagen,
                          Descripcion = r.Descripcion,
                          Ingredientes = r.Ingredientes,
                          Preparacion = r.Preparacion
                      })
                      .FirstOrDefaultAsync();
        public async Task<IEnumerable<RecestasDtos>> GetAllAsync()
        {
            return await _ctx.Recetas
                .Select(r => new RecestasDtos
                {
                    Id_Recetas = r.Id_Recetas,
                    Nombre = r.Nombre,
                    Categoria = r.Categoria,
                    Tiempo_preparacion = r.Tiempo_preparacion,
                    Imagen = r.Imagen,
                    Descripcion = r.Descripcion,
                    Ingredientes = r.Ingredientes,
                    Preparacion = r.Preparacion
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<RecestasDtos>> GetByCategoryAsync(string categoria) =>
            await _ctx.Recetas
                      .Where(r => r.Categoria == categoria)          // <-- filtro
                      .AsNoTracking()
                      .Select(r => new RecestasDtos                  // <-- tu DTO
                      {
                          Id_Recetas = r.Id_Recetas,
                          Nombre = r.Nombre,
                          Categoria = r.Categoria,
                          Tiempo_preparacion = r.Tiempo_preparacion,
                          Imagen = r.Imagen,
                          Descripcion = r.Descripcion,
                          Ingredientes = r.Ingredientes,
                          Preparacion = r.Preparacion
                      })
                      .ToListAsync();

        public static async Task SeedAsync(ApplicationDbContext ctx)
        {
            // Crea la BD si no existe
            await ctx.Database.EnsureCreatedAsync();

            // LOG 1 – vemos la BD exacta que está usando
            Console.WriteLine("▶ Seeder → BD: " + ctx.Database.GetDbConnection().ConnectionString);

            // Contamos registros
            var existentes = await ctx.Recetas.CountAsync();
            Console.WriteLine($"▶ Recetas existentes: {existentes}");

            // Si ya hay algo salimos
            if (existentes > 0) return;
            const string UploadPath = "Uploads/";   // ← mayúscula
            var recetas = new List<Receta>
        {
            new Receta
                {
                    Nombre = "Ensalada de Salmón",
                    Categoria = "Ensaladas",
                    Tiempo_preparacion = "15 minutos",
                    Imagen = UploadPath + "Ensalmon.png",
                    Descripcion = "Ensalada fresca con salmón, lechuga, pepino y aderezo cítrico.",
                    Ingredientes = "Salmón fresco; lechuga mixta; pepino; cebolla morada; eneldo; limón; aceite de oliva; sal; pimienta",
                    Preparacion = "Lavar hojas; añadir pepino y cebolla; incorporar salmón; espolvorear eneldo; mezclar jugo de limón con aceite, sal y pimienta; aliñar y mezclar suavemente."
                },
                new Receta
                {
                    Nombre = "Ensalada de Pollo y Aguacate",
                    Categoria = "Ensaladas",
                    Tiempo_preparacion = "20 minutos",
                    Imagen = UploadPath + "EnpolloA.webp",
                    Descripcion = "Ensalada nutritiva con pollo a la plancha, hojas verdes, aguacate y tomates cherry.",
                    Ingredientes = "Pechuga de pollo; mezcla de hojas verdes; aguacate; tomates cherry; aceite de oliva; jugo de limón; sal; pimienta; ajo en polvo; orégano",
                    Preparacion = "Sazonar y cocinar el pollo; cortar aguacate y tomates; mezclar con hojas verdes; aderezar con aceite y limón; servir."
                },
                new Receta
                {
                    Nombre = "Ensalada de Quinoa y Aguacate",
                    Categoria = "Ensaladas",
                    Tiempo_preparacion = "25 minutos",
                    Imagen = UploadPath + "enquinoaA.webp",
                    Descripcion = "Bol saludable de quinoa, batata asada, kale y aguacate.",
                    Ingredientes = "Quinoa; batata; kale o espinaca; aguacate; rábanos; almendras fileteadas; semillas; aceite de oliva; jugo de limón; comino; sal; pimienta",
                    Preparacion = "Cocer quinoa; asar batata con comino; mezclar con kale, aguacate y rábanos; añadir frutos secos y aliño de limón y aceite."
                },
                new Receta
                {
                    Nombre = "Ensalada César con Pollo",
                    Categoria = "Ensaladas",
                    Tiempo_preparacion = "25 minutos",
                    Imagen = UploadPath + "EncesarP.webp",
                    Descripcion = "Clásica César con pollo a la plancha, croutons y aderezo cremoso.",
                    Ingredientes = "Pechuga de pollo; lechuga romana; pan; queso parmesano; yema de huevo; aceite de oliva; jugo de limón; ajo; mostaza; salsa inglesa; sal; pimienta",
                    Preparacion = "Cocinar pollo; tostar cubos de pan; batir aderezo César; mezclar con lechuga; añadir pollo, croutons y parmesano."
                },
                new Receta
                {
                    Nombre = "Ensalada de Patatas Asadas",
                    Categoria = "Ensaladas",
                    Tiempo_preparacion = "35 minutos",
                    Imagen = UploadPath + "EnPotato.webp",
                    Descripcion = "Patatas al horno con espinacas, cebolla morada y aderezo de yogur.",
                    Ingredientes = "Patatas pequeñas; aceite de oliva; romero; sal; pimienta; espinacas; cebolla morada; queso feta; yogur; jugo de limón; mostaza; ajo",
                    Preparacion = "Asar patatas con romero; preparar aderezo de yogur; combinar con espinacas, cebolla, feta y patatas; aliñar."
                },
                new Receta
                {
                    Nombre = "Ensalada de Calabaza Asada",
                    Categoria = "Ensaladas",
                    Tiempo_preparacion = "30 minutos",
                    Imagen = UploadPath + "Encalabaza.webp",
                    Descripcion = "Calabaza rostizada con feta y granada.",
                    Ingredientes = "Calabaza; aceite de oliva; sal; pimienta; queso feta; granos de granada",
                    Preparacion = "Asar calabaza; colocar en plato; añadir feta y granada; servir tibia o fría."
                },

                // ────────────────────────  SOPAS Y CREMAS  ──────────────────────
                new Receta
                {
                    Nombre = "Sopa de Hongos",
                    Categoria = "Sopas y Cremas",
                    Tiempo_preparacion = "40 minutos",
                    Imagen = UploadPath + "SopOnos.webp",
                    Descripcion = "Sopa cremosa de champiñones con toque de perejil.",
                    Ingredientes = "Champiñones; mantequilla; cebolla; ajo; caldo; crema de leche; harina; sal; pimienta; perejil",
                    Preparacion = "Saltear cebolla, ajo y champiñones; agregar caldo; cocer; licuar; añadir crema; ajustar sazón y decorar."
                },
                new Receta
                {
                    Nombre = "Sopa de Calabaza y Coco",
                    Categoria = "Sopas y Cremas",
                    Tiempo_preparacion = "40 minutos",
                    Imagen = UploadPath + "Sopcalabaza.webp",
                    Descripcion = "Crema de calabaza con leche de coco y jengibre.",
                    Ingredientes = "Calabaza; jengibre; caldo de verduras; leche de coco; aceite de oliva; sal; pimienta; canela",
                    Preparacion = "Saltear jengibre; cocer calabaza con caldo; licuar; agregar leche de coco; sazonar con canela opcional."
                },
                new Receta
                {
                    Nombre = "Sopa Cremosa de Tomate",
                    Categoria = "Sopas y Cremas",
                    Tiempo_preparacion = "35 minutos",
                    Imagen = UploadPath + "SopTomate.webp",
                    Descripcion = "Tomate asado licuado con caldo y crema.",
                    Ingredientes = "Tomates; caldo; crema de leche; mantequilla; aceite de oliva; sal; pimienta; azúcar; queso parmesano",
                    Preparacion = "Asar tomates; saltear con mantequilla; añadir caldo; licuar; incorporar crema; ajustar sabores; servir con parmesano."
                },
                new Receta
                {
                    Nombre = "Sopa Cremosa de Salmón",
                    Categoria = "Sopas y Cremas",
                    Tiempo_preparacion = "40 minutos",
                    Imagen = UploadPath + "SopSalmon.png",
                    Descripcion = "Sopa de salmón con papas, zanahoria y eneldo.",
                    Ingredientes = "Salmón; cebolla; zanahoria; papa; ajo; caldo de pescado; crema de leche; eneldo; mantequilla; sal; pimienta",
                    Preparacion = "Sofreír verduras; verter caldo; cocer papas; añadir salmón y crema; sazonar y espolvorear eneldo."
                },
                new Receta
                {
                    Nombre = "Sopa de Fideos con Albóndigas",
                    Categoria = "Sopas y Cremas",
                    Tiempo_preparacion = "45 minutos",
                    Imagen = UploadPath + "SopAlbondo.webp",
                    Descripcion = "Caldo con albóndigas caseras, fideos y espinacas.",
                    Ingredientes = "Carne molida; huevo; pan rallado; ajo; comino; caldo; fideos; pasta de tomate; salsa de soya; jengibre; espinacas",
                    Preparacion = "Formar albóndigas; hervir caldo con pasta de tomate y especias; cocinar albóndigas; añadir fideos y espinacas; servir."
                },

                // ───────────────────────────  PASTAS  ───────────────────────────
                new Receta
                {
                    Nombre = "Espaguetis con Tomates Cherry",
                    Categoria = "Pastas",
                    Tiempo_preparacion = "25 minutos",
                    Imagen = UploadPath + "PastTOmate.webp",
                    Descripcion = "Espaguetis al ajo con tomates cherry y albahaca.",
                    Ingredientes = "Espaguetis; tomates cherry; ajo; aceite de oliva; sal; pimienta; hojuelas de chile; albahaca; queso parmesano",
                    Preparacion = "Cocer pasta; saltear ajo y tomates; agregar pasta y agua de cocción; sazonar; terminar con albahaca y parmesano."
                },
                new Receta
                {
                    Nombre = "Rigatoni con Salsa de Crema",
                    Categoria = "Pastas",
                    Tiempo_preparacion = "30 minutos",
                    Imagen = UploadPath + "PastRigatono.webp",
                    Descripcion = "Rigatoni en salsa cremosa de champiñones y parmesano.",
                    Ingredientes = "Rigatoni; crema de leche; mantequilla; queso parmesano; ajo; cebolla; caldo; champiñones; sal; pimienta",
                    Preparacion = "Cocer rigatoni; saltear cebolla, ajo y champiñones; reducir con caldo; añadir crema y parmesano; mezclar pasta."
                },
                new Receta
                {
                    Nombre = "Fusilli con Aceitunas y Tomates",
                    Categoria = "Pastas",
                    Tiempo_preparacion = "25 minutos",
                    Imagen = UploadPath + "PastOlio.webp",
                    Descripcion = "Fusilli mediterráneo con aceitunas negras y tomates cherry.",
                    Ingredientes = "Fusilli; aceitunas negras; tomates cherry; ajo; albahaca; aceite de oliva; queso parmesano; sal; pimienta",
                    Preparacion = "Cocer fusilli; saltear ajo y tomates; añadir aceitunas; integrar pasta; terminar con albahaca y parmesano."
                },
                new Receta
                {
                    Nombre = "Linguine con Camarones",
                    Categoria = "Pastas",
                    Tiempo_preparacion = "30 minutos",
                    Imagen = UploadPath + "PastLinguini.jpg",
                    Descripcion = "Linguine en salsa de tomate ligera con camarones.",
                    Ingredientes = "Linguine; camarones; tomates cherry; ajo; albahaca; salsa de tomate; queso parmesano; aceite de oliva; sal; pimienta",
                    Preparacion = "Cocer linguine; saltear ajo y camarones; añadir tomates y salsa; mezclar pasta; sazonar y servir con albahaca y queso."
                },
                new Receta
                {
                    Nombre = "Penne con Parmesano",
                    Categoria = "Pastas",
                    Tiempo_preparacion = "35 minutos",
                    Imagen = UploadPath + "PastPenne.webp",
                    Descripcion = "Penne en salsa de carne y tomate coronado con parmesano.",
                    Ingredientes = "Penne; carne molida; cebolla; ajo; salsa de tomate; caldo; aceite de oliva; queso parmesano; perejil; sal; pimienta; orégano; albahaca",
                    Preparacion = "Cocer penne; dorar carne con cebolla y ajo; agregar salsa y hierbas; mezclar pasta; espolvorear parmesano y perejil."
                },

                // ───────────────────────────  POSTRES  ──────────────────────────
                new Receta
                {
                    Nombre = "Panqueques",
                    Categoria = "Postres",
                    Tiempo_preparacion = "20 minutos",
                    Imagen = UploadPath + "postPankake.jpg",
                    Descripcion = "Panqueques esponjosos con arándanos y miel.",
                    Ingredientes = "Harina; polvo de hornear; azúcar; sal; leche; huevo; mantequilla; vainilla; arándanos; miel",
                    Preparacion = "Mezclar secos; batir líquidos; unir masas; cocer porciones en sartén; servir con arándanos y miel."
                },
                new Receta
                {
                    Nombre = "Crumble de Frutos Rojos",
                    Categoria = "Postres",
                    Tiempo_preparacion = "25 minutos",
                    Imagen = UploadPath + "PostreRD.png",
                    Descripcion = "Frutos rojos horneados con cobertura crujiente de avena.",
                    Ingredientes = "Frutos rojos variados; azúcar; jugo de limón; harina; mantequilla fría; azúcar morena; avena; canela",
                    Preparacion = "Mezclar frutas con azúcar y limón; cubrir con masa crumble; hornear hasta dorar; servir tibio."
                },
                new Receta
                {
                    Nombre = "Crème Brûlée",
                    Categoria = "Postres",
                    Tiempo_preparacion = "90 minutos",
                    Imagen = UploadPath + "PostrCreem.webp",
                    Descripcion = "Clásico postre francés con costra de azúcar caramelizado.",
                    Ingredientes = "Crema para batir; yemas de huevo; azúcar; vainilla; azúcar extra para caramelizar",
                    Preparacion = "Calentar crema con vainilla; batir yemas con azúcar; temperar; hornear en baño maría; enfriar; caramelizar azúcar."
                },
                new Receta
                {
                    Nombre = "Waffles con Frutos Rojos",
                    Categoria = "Postres",
                    Tiempo_preparacion = "25 minutos",
                    Imagen = UploadPath + "PosWafleRes.jpg",
                    Descripcion = "Waffles crujientes servidos con yogur, frutos rojos y miel.",
                    Ingredientes = "Harina; leche; huevos; mantequilla; azúcar; polvo de hornear; vainilla; sal; frutos rojos; miel",
                    Preparacion = "Mezclar secos y líquidos; cocer en waflera; servir con yogur, frutas y miel; espolvorear azúcar glas."
                },

                // ────────────────────────────  BOWLS  ───────────────────────────
                new Receta
                {
                    Nombre = "Bibimbap",
                    Categoria = "Bowls",
                    Tiempo_preparacion = "30 minutos",
                    Imagen = UploadPath + "BlowBliblap.png",
                    Descripcion = "Bowl coreano de arroz con verduras, carne y huevo.",
                    Ingredientes = "Arroz; carne de res; zanahoria; pepino; brotes de soja; espinacas; huevo; ajo; aceite de sésamo; semillas de sésamo; gochujang; salsa de soya; miel",
                    Preparacion = "Saltear verduras; dorar carne con ajo; freír huevo; mezclar salsa gochujang; montar ingredientes sobre arroz; servir."
                },
                new Receta
                {
                    Nombre = "Cerdo Crujiente a la Parrilla con Arroz",
                    Categoria = "Bowls",
                    Tiempo_preparacion = "90 minutos",
                    Imagen = UploadPath + "BlowCerdo.webp",
                    Descripcion = "Panceta marinada y crujiente servida sobre arroz jazmín.",
                    Ingredientes = "Panceta de cerdo; salsa de soya; miel; ajo; jengibre; salsa de ostras; aceite de sésamo; vinagre de arroz; sal gruesa; arroz jazmín; cebollín; sésamo",
                    Preparacion = "Marinar cerdo; secar y salar piel; asar en parrilla hasta crujir; cocer arroz; servir rebanadas de cerdo sobre arroz y decorar."
                },
                new Receta
                {
                    Nombre = "Tazón de Sopa Ramen",
                    Categoria = "Bowls",
                    Tiempo_preparacion = "45 minutos",
                    Imagen = UploadPath + "Blowramen.webp",
                    Descripcion = "Ramen casero con caldo aromático, fideos y toppings."
                    ,
                    Ingredientes = "Caldo; ajo; jengibre; salsa de soya; miso; mirin; fideos ramen; pechuga de pollo; huevos; cebollín; hongos; zanahoria; algas",
                    Preparacion = "Sofreír aromáticos; hervir caldo con condimentos; cocer pollo y huevos; cocinar fideos; montar tazón con toppings; añadir caldo."
                },
                new Receta
                {
                    Nombre = "Arroz Frito con Huevo y Cebolleta",
                    Categoria = "Bowls",
                    Tiempo_preparacion = "20 minutos",
                    Imagen = UploadPath + "BlowarrosF.webp",
                    Descripcion = "Arroz salteado al estilo asiático con huevo revuelto y cebolleta.",
                    Ingredientes = "Arroz cocido; huevos; cebolleta; ajo; salsa de soya; aceite vegetal; aceite de sésamo; sal; pimienta; azúcar",
                    Preparacion = "Saltear huevos revueltos; dorar ajo y cebolleta; añadir arroz frío; sazonar con soya, pimienta y aceite de sésamo; incorporar huevos y partes verdes de cebolleta."
                }
            // 👉 Añade aquí las DEMÁS recetas (o empieza con 1 para probar)
        };

            // Guardamos
            await ctx.Recetas.AddRangeAsync(recetas);
            await ctx.SaveChangesAsync();

            Console.WriteLine($"✔ Se insertaron {recetas.Count} recetas");
        }

    }
}
