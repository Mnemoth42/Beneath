using TkrainDesigns.Stats;
using UnityEngine;
#pragma warning disable CS0649
namespace RPG.Stats.Modifiers
{
    public class RandomStatDecoratorProvider : MonoBehaviour
    {
        RandomStatDecorator decorator = null;



        public RandomStatDecorator GetDecorator()
        {
            return decorator;
        }

        public RandomStatDecorator CreateDecorator(int level)
        {
            if (level < 1) level = 1;
            //decorator = new RandomStatDecorator(level);
            return decorator;
        }

        public RandomStatDecorator AssignDecorator(RandomStatDecorator randomStatDecorator)
        {
            decorator = randomStatDecorator;
            return decorator;
        }
    }
}