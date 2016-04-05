using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bottles
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Set first bottles capacity:");
                var cap1 = Console.ReadLine();
                var bottle1 = new Bottle(Convert.ToInt32(cap1));

                Console.WriteLine();
                Console.WriteLine("Set second bottles capacity:");
                var cap2 = Console.ReadLine();
                var bottle2 = new Bottle(Convert.ToInt32(cap2));

                Console.WriteLine();
                Console.WriteLine("Set target value:");
                var targ = Console.ReadLine();
                var target = Convert.ToInt32(targ);


                if (target > bottle1.Capacity && target > bottle2.Capacity || target <= 0) throw new ArgumentException("Unreachable target value");

                var path = new List<Tuple<string, int, int>>();
                path.Add(new Tuple<string, int, int>("Start. " + bottle1.Name + ": " + bottle1.Ammount + "/" + bottle1.Capacity + ". "
                        + bottle2.Name + ": " + bottle2.Ammount + "/" + bottle2.Capacity + ". ", 0, 0));

                path = FindPath(bottle1, bottle2, target, path);

                Console.WriteLine();
                foreach (var step in path)
                {
                    Console.WriteLine(step.Item1);
                }

                Console.ReadLine();
            }
        }

        public static List<Tuple<string, int, int>> FindPath(Bottle bottle1, Bottle bottle2, int target, List<Tuple<string, int, int>> path)
        {
            if (bottle1.Ammount == target || bottle2.Ammount == target)
                return path;

            var bestPath = new List<Tuple<string, int, int>>(path);
            var subSteps = Int32.MaxValue;
            var bottle1StartAmmount = bottle1.Ammount;
            var bottle2StartAmmount = bottle2.Ammount;

            //Never fill a bottle if the other is full
            if (!bottle2.IsFull() && bottle1.Fill())
            {
                //Check if we have a duplicate state. Only proceed if we are in a unique state
                var ammount1 = bottle1.Ammount;
                var ammount2 = bottle2.Ammount;
                if(!path.Exists(x => x.Item2 == ammount1 && x.Item3 == ammount2))
                {
                    //Copy path
                    var subPath = new List<Tuple<string, int, int>>(path);
                    subPath.Add(new Tuple<string, int, int>("Fill " + bottle1.Name + ". " + bottle1.Name + ": " + bottle1.Ammount + "/" + bottle1.Capacity + ". "
                        + bottle2.Name + ": " + bottle2.Ammount + "/" + bottle2.Capacity + ". ", bottle1.Ammount, bottle2.Ammount));

                    //Continue recursive search
                    var p = FindPath(bottle1, bottle2, target, subPath);
                    if (p.Count < subSteps)
                    {
                        subSteps = p.Count;
                        bestPath = p;
                    }
                }
            }

            //Reset bottles for new branch
            bottle1.Ammount = bottle1StartAmmount;
            bottle2.Ammount = bottle2StartAmmount;

            if (!bottle1.IsFull() && bottle2.Fill())
            {
                var ammount1 = bottle1.Ammount;
                var ammount2 = bottle2.Ammount;
                if (!path.Exists(x => x.Item2 == ammount1 && x.Item3 == ammount2))
                {
                    var subPath = new List<Tuple<string, int, int>>(path);
                    subPath.Add(new Tuple<string, int, int>("Fill " + bottle2.Name + ". " + bottle1.Name + ": " + bottle1.Ammount + "/" + bottle1.Capacity + ". "
                    + bottle2.Name + ": " + bottle2.Ammount + "/" + bottle2.Capacity + ". ", bottle1.Ammount, bottle2.Ammount));

                    var p = FindPath(bottle1, bottle2, target, subPath);
                    if (p.Count < subSteps)
                    {
                        subSteps = p.Count;
                        bestPath = p;
                    }
                }
            }

            bottle1.Ammount = bottle1StartAmmount;
            bottle2.Ammount = bottle2StartAmmount;

            //Never empty a bottle if the other is empty
            if (bottle2.Ammount != 0 && bottle1.Pour())
            {
                var ammount1 = bottle1.Ammount;
                var ammount2 = bottle2.Ammount;
                if (!path.Exists(x => x.Item2 == ammount1 && x.Item3 == ammount2))
                {
                    var subPath = new List<Tuple<string, int, int>>(path);
                    subPath.Add(new Tuple<string, int, int>("Pour " + bottle1.Name + ". " + bottle1.Name + ": " + bottle1.Ammount + "/" + bottle1.Capacity + ". "
                        + bottle2.Name + ": " + bottle2.Ammount + "/" + bottle2.Capacity + ". ", bottle1.Ammount, bottle2.Ammount));

                    var p = FindPath(bottle1, bottle2, target, subPath);
                    if (p.Count < subSteps)
                    {
                        subSteps = p.Count;
                        bestPath = p;
                    }
                }
            }

            bottle1.Ammount = bottle1StartAmmount;
            bottle2.Ammount = bottle2StartAmmount;

            if (bottle2.Ammount != 0 &&  bottle2.Pour())
            {
                var ammount1 = bottle1.Ammount;
                var ammount2 = bottle2.Ammount;
                if (!path.Exists(x => x.Item2 == ammount1 && x.Item3 == ammount2))
                {
                    var subPath = new List<Tuple<string, int, int>>(path);
                    subPath.Add(new Tuple<string, int, int>("Pour " + bottle2.Name + ". " + bottle1.Name + ": " + bottle1.Ammount + "/" + bottle1.Capacity + ". "
                    + bottle2.Name + ": " + bottle2.Ammount + "/" + bottle2.Capacity + ". ", bottle1.Ammount, bottle2.Ammount));

                    var p = FindPath(bottle1, bottle2, target, subPath);
                    if (p.Count < subSteps)
                    {
                        subSteps = p.Count;
                        bestPath = p;
                    }
                }
            }

            bottle1.Ammount = bottle1StartAmmount;
            bottle2.Ammount = bottle2StartAmmount;

            if (bottle1.Transfer(ref bottle2))
            {
                var ammount1 = bottle1.Ammount;
                var ammount2 = bottle2.Ammount;
                if (!path.Exists(x => x.Item2 == ammount1 && x.Item3 == ammount2))
                {
                    var subPath = new List<Tuple<string, int, int>>(path);
                    subPath.Add(new Tuple<string, int, int>("Transfer " + bottle1.Name + " into " + bottle2.Name + ". " + bottle1.Name + ": " + bottle1.Ammount + "/" + bottle1.Capacity + ". "
                    + bottle2.Name + ": " + bottle2.Ammount + "/" + bottle2.Capacity + ". ", bottle1.Ammount, bottle2.Ammount));

                    var p = FindPath(bottle1, bottle2, target, subPath);
                    if (p.Count < subSteps)
                    {
                        subSteps = p.Count;
                        bestPath = p;
                    }
                }
            }

            bottle1.Ammount = bottle1StartAmmount;
            bottle2.Ammount = bottle2StartAmmount;

            if (bottle2.Transfer(ref bottle1))
            {
                var ammount1 = bottle1.Ammount;
                var ammount2 = bottle2.Ammount;
                if (!path.Exists(x => x.Item2 == ammount1 && x.Item3 == ammount2))
                {
                    var subPath = new List<Tuple<string, int, int>>(path);
                    subPath.Add(new Tuple<string, int, int>("Transfer " + bottle2.Name + " into " + bottle1.Name + ". " + bottle1.Name + ": " + bottle1.Ammount + "/" + bottle1.Capacity + ". "
                    + bottle2.Name + ": " + bottle2.Ammount + "/" + bottle2.Capacity + ". ", bottle1.Ammount, bottle2.Ammount));

                    var p = FindPath(bottle1, bottle2, target, subPath);
                    if (p.Count < subSteps)
                    {
                        subSteps = p.Count;
                        bestPath = p;
                    }
                }
            }

            return bestPath;
        }
    }
    public class Bottle
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Ammount { get; set; }
        public Bottle(int cap)
        {
            if(cap <= 0)
            {
                throw new ArgumentException("Capacity must be greater than 0");
            }

            Name = "Bottle" + cap;
            Capacity = cap;
        }
        public bool IsFull()
        {
            return Ammount == Capacity;
        }
        public bool Fill()
        {
            //Bottle is full
            if (Ammount == Capacity) return false;
            Ammount = Capacity;
            return true;
        }
        public bool Pour()
        {
            //Bottle is empty
            if (Ammount == 0) return false;
            Ammount = 0;
            return true;
        }
        public bool Transfer(ref Bottle target)
        {
            //Target bottle is full
            if (target.Ammount == target.Capacity) return false;
            //Source bottle is empty
            if (Ammount == 0) return false;

            var dif = target.Capacity - target.Ammount;

            //Difference in target is greater than the ammount in source. Empty the source into target
            if (dif >= Ammount)
            {               
                target.Ammount += Ammount;
                Ammount = 0;
            }
            //Difference in target is less than ammount in source. Fill target and remove that much from source.
            else if (dif < Ammount)
            {
                Ammount -= dif;
                target.Ammount += dif;
            }

            return true;
        }
    }
}
