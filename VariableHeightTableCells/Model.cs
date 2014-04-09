using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;

namespace VariableHeightTableCells
{
    public class Model
    {
        private static readonly string loremIpsumText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent non quam ac massa viverra semper. Maecenas mattis justo ac augue volutpat congue. Maecenas laoreet, nulla eu faucibus gravida, felis orci dictum risus, sed sodales sem eros eget risus. Morbi imperdiet sed diam et sodales. Vestibulum ut est id mauris ultrices gravida. Nulla malesuada metus ut erat malesuada, vitae ornare neque semper. Aenean a commodo justo, vel placerat odio. Curabitur vitae consequat tortor. Aenean eu magna ante. Integer tristique elit ac augue laoreet, eget pulvinar lacus dictum. Cras eleifend lacus eget pharetra elementum. Etiam fermentum eu felis eu tristique. Integer eu purus vitae turpis blandit consectetur. Nulla facilisi. Praesent bibendum massa eu metus pulvinar, quis tristique nunc commodo. Ut varius aliquam elit, a tincidunt elit aliquam non. Nunc ac leo purus. Proin condimentum placerat ligula, at tristique neque scelerisque ut. Suspendisse ut congue enim. Integer id sem nisl. Nam dignissim, lectus et dictum sollicitudin, libero augue ullamcorper justo, nec consectetur dolor arcu sed justo. Proin rutrum pharetra lectus, vel gravida ante venenatis sed. Mauris lacinia urna vehicula felis aliquet venenatis. Suspendisse non pretium sapien. Proin id dolor ultricies, dictum augue non, euismod ante. Vivamus et luctus augue, a luctus mi. Maecenas sit amet felis in magna vestibulum viverra vel ut est. Suspendisse potenti. Morbi nec odio pretium lacus laoreet volutpat sit amet at ipsum. Etiam pretium purus vitae tortor auctor, quis cursus metus vehicula. Integer ultricies facilisis arcu, non congue orci pharetra quis. Vivamus pulvinar ligula neque, et vehicula ipsum euismod quis.";
        private static readonly string[] loremIpsumParts = loremIpsumText.Split(' ').ToArray();
        private static readonly Random random = new Random();

        private readonly List<Item> items;

        public Model()
        {
            this.items = new List<Item>();
        }

        public IList<Item> Items
        {
            get { return this.items; }
        }

        public void PopulateItems()
        {
            this.items.AddRange(UIFont.FamilyNames.Select(x => new Item { Title = x, Body = GenerateRandomLoremIpsum() }));
        }

        public void AddSingleItem()
        {
            var title = UIFont.FamilyNames[random.Next(UIFont.FamilyNames.Length)];
            this.items.Add(new Item { Title = title, Body = GenerateRandomLoremIpsum() });
        }
            
        private static string GenerateRandomLoremIpsum()
        {
            var wordCount = random.Next(3, loremIpsumParts.Length);

            return loremIpsumParts
                .Take(wordCount)
                .Aggregate(new StringBuilder(), (sb, s) => sb.Append(s).Append(" "))
                .ToString();
        }
    }
}