using System.Linq;
using UnityEngine;

namespace TkrainDesigns.Tiles.Control
{
    public static class ControllerCoordinator
    {
        public static void BeginNextControllerTurn()
        {
            var controller = Object.FindObjectsOfType<BaseController>().Where(h=>h.IsAlive).OrderBy(t=>t.NextTurn).First();
            controller.BeginTurn();
        }

    }
}