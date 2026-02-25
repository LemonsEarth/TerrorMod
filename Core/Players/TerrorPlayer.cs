using Terraria.Graphics.Effects;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using System.IO;
using TerrorMod.Content.NPCs.Bosses;
using TerrorMod.Content.Buffs.Debuffs.Misc;

namespace TerrorMod.Core.Players;

public class TerrorPlayer : ModPlayer
{
    public static int Timer { get; private set; } = 0;
    public bool undeadAmulet = false;

    public int curseLevel = 0;

    public override void ResetEffects()
    {
        undeadAmulet = false;
    }

    public override void PostUpdate()
    {
        if (Main.netMode == NetmodeID.Server) return;
        if (curseLevel > 0)
        {
            Player.AddBuff(BuffType<CurseDebuff>(), 2);
        }

        if (Timer % 60 == 0 && Filters.Scene["TerrorMod:DesaturateShader"].IsActive() && !NPC.AnyNPCs(NPCType<InfiniteTerrorCage>()))
        {
            Filters.Scene.Deactivate("TerrorMod:DesaturateShader");
        }

        if (Timer % 60 == 0)
        {
            if (!NPC.AnyNPCs(NPCType<InfiniteTerrorHead>()))
            {
                if (SkyManager.Instance["TerrorMod:TerrorSky"].IsActive())
                {
                    SkyManager.Instance.Deactivate("TerrorMod:TerrorSky");
                }
            }
        }

        Timer++;
    }

    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
        if (Player.difficulty == 0 && !undeadAmulet) curseLevel++;
    }

    public override void PostUpdateMiscEffects()
    {
        for (int i = 0; i < curseLevel; i++)
        {
            if (Player.statLifeMax2 > 50)
            {
                Player.statLifeMax2 -= 10;
            }
        }
    }

    public override void Initialize()
    {
        curseLevel = 0;
    }

    public override void SaveData(TagCompound tag)
    {
        tag["curseLevel"] = curseLevel;
    }

    public override void LoadData(TagCompound tag)
    {
        curseLevel = tag.GetInt("curseLevel");
    }

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        ModPacket packet = Mod.GetPacket();
        packet.Write((byte)TerrorMod.MessageType.CurseLevelSync);
        packet.Write((byte)Player.whoAmI);
        packet.Write((byte)curseLevel);
        packet.Send(toWho, fromWho);
    }

    public void ReceivePlayerSync(BinaryReader reader)
    {
        curseLevel = reader.ReadByte();
    }

    public override void CopyClientState(ModPlayer targetCopy)
    {
        TerrorPlayer clone = (TerrorPlayer)targetCopy;
        clone.curseLevel = curseLevel;
    }

    public override void SendClientChanges(ModPlayer clientPlayer)
    {
        TerrorPlayer clone = (TerrorPlayer)clientPlayer;

        if (clone.curseLevel != curseLevel)
        {
            SyncPlayer(-1, Main.myPlayer, false);
        }
    }
}