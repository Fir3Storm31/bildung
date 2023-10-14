using System.Numerics;
using Raylib_cs;

namespace Bildung
{
    public class Element
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public List<Element> Inputs { get; set; }

        public Element(Vector2 position, Texture2D texture, List<Element> inputs)
        {
            Position = position;
            Texture = texture;
            Inputs = inputs;
        }
    }

    // inherited element class InputData
    public class InputData : Element
    {
        public List<double> Values { get; set; }
        public List<double> Weights { get; set; }

        public InputData(Vector2 position, Texture2D texture, List<Element> inputs, List<double> values, List<double> weights) : base(position, texture, inputs)
        {
            Values = values;
            Weights = weights;
        }
    }

    // inherited element class Sum
    public class Sum : Element
    {
        public Sum(Vector2 position, Texture2D texture, List<Element> inputs) : base(position, texture, inputs)
        {
        }

        public double callFunction(List<double> values, List<double> weights)
        {
            double sum = 0;
            for (int i = 0; i < values.Count; i++)
            {
                sum += values[i] * weights[i];
            }
            return sum;
        }
    }
}
