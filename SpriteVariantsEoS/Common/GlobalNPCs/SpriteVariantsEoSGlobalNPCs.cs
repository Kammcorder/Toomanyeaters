using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpriteVariantsEoS.Common.GlobalNPCs
{
	public class EaterOfSoulsVariants : GlobalNPC
	{
		public override bool InstancePerEntity => true;

	private int variant;

public override void OnSpawn(NPC npc, IEntitySource source)
{
	if (npc.type == NPCID.EaterofSouls) {

		float chance = Main.rand.NextFloat();

		if (chance < 0.05f)
		{
			variant = 1; // 5% variant
		}
		else if (chance < 0.3f)
		{
			variant = 2; // 30% variant
		}
		else
		{
			variant = 0; // 65% normal
		}

		npc.netUpdate = true;
	}
}

		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (npc.type != NPCID.EaterofSouls)
				return true;

			if (variant == 0)
				return true; // draw normal vanilla sprite

			string path = variant switch
			{
				1 => "SpriteVariantsEoS/Assets/NPCs/EaterofSouls_1",
			    2 => "SpriteVariantsEoS/Assets/NPCs/EaterofSouls_2",
				_ => null
			};
			

			if (path == null)
				return true;

			Asset<Texture2D> asset = ModContent.Request<Texture2D>(path);
			Texture2D texture = asset.Value;

			SpriteEffects effects = npc.spriteDirection == 1
				? SpriteEffects.FlipHorizontally
				: SpriteEffects.None;

			Vector2 drawPos = npc.Center - screenPos + new Vector2(0f, npc.gfxOffY);
			Vector2 origin = npc.frame.Size() / 2f;

			spriteBatch.Draw(
				texture,
				drawPos,
				npc.frame,
				npc.GetAlpha(drawColor),
				npc.rotation,
				origin,
				npc.scale,
				effects,
				0f
			);

			return false; // skip vanilla only for custom variants
		}

		public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
		{
			if (npc.type == NPCID.EaterofSouls) {
				binaryWriter.Write((byte)variant);
			}
		}

		public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
		{
			if (npc.type == NPCID.EaterofSouls) {
				variant = binaryReader.ReadByte();
			}
		}
	}
}
