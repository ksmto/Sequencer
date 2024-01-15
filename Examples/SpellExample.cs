using ThunderRoad;
using UnityEngine;

namespace kSequencer.Examples;

public class SequencerSpell : SpellCastCharge
{
    // Initialize and start our sequencer
    private Sequencer _sequencer = Sequencer.Start();
    
    public override void Load(SpellCaster spellCaster, Level level)
    {
        base.Load(spellCaster, level);
        // Uses the Sequencer to check if we pressed the grip while we are casting the spell, then spawn an item into the players hand
        _sequencer.If(() => spellCaster.ragdollHand.playerHand.controlHand.gripPressed).While(() => spellCaster.isFiring).Do(SpawnItemInHand);
    }

    // Spawns an item into the players hand
    private void SpawnItemInHand()
    {
        // Spawn an item
        var itemData = Catalog.GetData<ItemData>("DaggerCommon");
        itemData?.SpawnAsync(item =>
        {
            // Grab the item
            spellCaster.ragdollHand.Grab(item.GetMainHandle(spellCaster.ragdollHand.side));
        }, spellCaster.magicSource.position, Quaternion.identity);
        
        // End the spell's cast
        spellCaster.Fire(false);
    }

    // Runs every frame, while the spell is active (casting or not)
    public override void UpdateCaster()
    {
        base.UpdateCaster();
        // Update the sequencer
        _sequencer.Update();
    }
}