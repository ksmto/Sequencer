using ThunderRoad;
using UnityEngine;

namespace kSequencer.Examples;

public class SequencerItemModule : ItemModule
{
    // When the item is loaded
    public override void OnItemLoaded(Item item)
    {
        base.OnItemLoaded(item);
        // Add our behaviour component
        item.gameObject.TryGetOrAddComponent(out MonoBehaviour behaviour);
    }
}

public class SequencerItemBehaviour : MonoBehaviour
{
    private Item _item;

    // Initialize and start our sequencer
    private readonly Sequencer _sequencer = Sequencer.Start();

    // When our item is spawned
    private void Start()
    {
        _item = GetComponent<Item>();

        if (_item != null && _item.mainHandler != null)
        {
            // Use the sequencer to check if we pressed the alternate use button while holding it, if so, despawn the item
            _sequencer?.If(() => _item.mainHandler.playerHand.controlHand.alternateUsePressed).Do(_item.Despawn);
        }
    }

    // Runs every frame of the game, while the item is spawned
    private void Update()
    {
        // Update the sequencer
        _sequencer.Update();
    }
}