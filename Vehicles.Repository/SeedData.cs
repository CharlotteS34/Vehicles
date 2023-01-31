using Vehicles.Repository.Entities;

namespace Vehicles.Repository
{
    internal static class SeedData
    {
        public static Make[] Makes = new Make[2] 
        {
            new Make { MakeId = 1, MakeName = "Volkswagen" },
            new Make { MakeId = 2, MakeName = "Audi" }
        };

        public static Model[] Models = new Model[4]
        {
            new Model { ModelId = 1, MakeId = 1, ModelName = "Golf" },
            new Model { ModelId = 2, MakeId = 1, ModelName = "Polo" },
            new Model { ModelId = 3, MakeId = 2, ModelName = "A4" },
            new Model { ModelId = 4, MakeId = 2, ModelName = "A6" }
        };

        public static Colour[] Colours = new Colour[5]
        {
            new Colour { ColourId = 1, ColourName = "Blue" },
            new Colour { ColourId = 2, ColourName = "Black" },
            new Colour { ColourId = 3, ColourName = "Red" },
            new Colour { ColourId = 4, ColourName = "Green" },            
            new Colour { ColourId = 5, ColourName = "Silver" },
        };
    }
}
