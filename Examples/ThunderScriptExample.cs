using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace kSequencer.Examples;

public class SequencerThunderScript : ThunderScript
{
    // Initialize and start our sequencer
    private readonly Sequencer _sequencer = Sequencer.Start();

    // When our script is enabled in the game
    public override void ScriptEnable()
    {
        base.ScriptEnable();
        _sequencer.If(() => Player.currentCreature != null).Do(() => GameManager.local.StartCoroutine(SpawnItemPerSecond()));
    }

    // Spawns an item in the position of the players left hand magic position, per second the player is active
    private IEnumerator SpawnItemPerSecond()
    {
        while (Player.currentCreature != null)
        {
            // Spawn an item
            var itemData = Catalog.GetData<ItemData>("DaggerCommon");
            itemData?.SpawnAsync(null, Player.currentCreature.handLeft.caster.magicSource.position, Quaternion.identity);

            // Wait 1 second
            yield return new WaitForSeconds(1.0f);
        }
    }

    // Runs every frame, while our script is active
    public override void ScriptUpdate()
    {
        base.ScriptUpdate();
        // Update the sequencer
        _sequencer.Update();
    }
}