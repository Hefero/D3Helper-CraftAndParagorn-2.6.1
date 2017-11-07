using Enigma.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enigma.D3.Collections;
using Enigma.D3.UI;
using Enigma.D3.Memory;

namespace Enigma.D3
{
	public partial class ObjectManager : MemoryObject
	{
		public const int SizeOf = 0xA60;

		public Time x000_Time { get { return Read<Time>(0x000); } }
		public int _x00C { get { return Read<int>(0x00C); } }
		public int _x010 { get { return Read<int>(0x010); } }
		public int _x014 { get { return Read<int>(0x014); } }
		public int x018 { get { return Read<int>(0x018); } }
		public int x01C { get { return Read<int>(0x01C); } }
		public Time x020_StructStart_Min24Bytes_Time_ { get { return Read<Time>(0x020); } }
		public int _x02C { get { return Read<int>(0x02C); } }
		public int _x030 { get { return Read<int>(0x030); } }
		public int _x034 { get { return Read<int>(0x034); } }
		public int x038_Counter_CurrentFrame { get { return Read<int>(0x038); } }
		public int x03C { get { return Read<int>(0x03C); } }
		// ...
		// Updated for 2.5.0
		public GameGlobals x068_GameGlobals { get { return Read<GameGlobals>(0x068); } }
		public GameGlobals x418_GameGlobals { get { return Read<GameGlobals>(0x418); } }
        // 8 bytes gone in 2.6.0, not sure what fields
		//public int _x7C8 { get { return Read<int>(0x798); } }
		//public int _x7CC { get { return Read<int>(0x79C); } }
		//public int _x7D0 { get { return Read<int>(0x7A0); } }
		//public int _x7D4 { get { return Read<int>(0x7A4); } }
		public Ptr<X790> x7D8_Ptr_292Bytes { get { return ReadPointer<X790>(0x7D8); } }
		//public int _x7DC_int { get { return Read<int>(0x7AC); } }
		//public int _x7E0 { get { return Read<int>(0x7B0); } }
		//public int _x7E4 { get { return Read<int>(0x7B4); } }
		public Storage x7E0_Storage { get { return Read<Storage>(0x7E0); } }
        public int _x98C { get { return Read<int>(0x98C); } }
        public int _x990 { get { return Read<int>(0x990); } }
        public Ptr<Physics> x994_Ptr_908Bytes_Physics { get { return ReadPointer<Physics>(0x994); } }
        public int _x998 { get { return Read<int>(0x998); } }
        public Ptr<ExpandableContainer> x99C_Ptr_Lights { get { return ReadPointer<ExpandableContainer>(0x99C); } }
        public Ptr<Container> x9A0_Ptr_Cutscenes { get { return ReadPointer<Container>(0x9A0); } }
        public Ptr<LightManager> x9A4_Ptr_92Bytes_LightManager { get { return ReadPointer<LightManager>(0x9A4); } }
        public Ptr<ExpandableContainer<Actor>> x9A8_Ptr_RActors { get { return ReadPointer<ExpandableContainer<Actor>>(0x9A8); } }
        public Ptr<ActorManager> x9AC_Ptr_816Bytes_ActorManager { get { return ReadPointer<ActorManager>(0x9AC); } }
        public Ptr<ClothManager> x9B0_Ptr_24Bytes_ClothManager { get { return ReadPointer<ClothManager>(0x9B0); } }
        public Ptr x9B4_Ptr_41484Bytes_Explosions { get { return Read<Ptr>(0x9B4); } }
        public int _x9B8_int { get { return Read<int>(0x9B8); } }
        public Ptr<TrailManager> x9BC_Ptr_12Bytes_TrailManager { get { return ReadPointer<TrailManager>(0x9BC); } }
        public Ptr<RopeManager> x9C0_Ptr_16Bytes_RopeManager { get { return ReadPointer<RopeManager>(0x9C0); } }
        public Ptr<Container> x9C4_Ptr_ParticleSystems { get { return ReadPointer<Container>(0x9C4); } }
        public Ptr x9C8_Ptr_76688Bytes_Particles { get { return Read<Ptr>(0x9C8); } }
        public Ptr<Cinematography> x9CC_Cinematography { get { return ReadPointer<Cinematography>(0x9CC); } }
        public Ptr<Container> x9D0_Ptr_AmbientSound { get { return ReadPointer<Container>(0x9D0); } }
        public Ptr<Ptr<Container>> x9D4_PtrPtr_Attachments { get { return ReadPointer<Ptr<Container>>(0x9D4); } }
        public Ptr<ParentObjects> x9D8_Ptr_ParentObjects { get { return ReadPointer<ParentObjects>(0x9D8); } }
        public Ptr<Ptr<Container>> x9DC_PtrPtr_DelayedRMessages { get { return ReadPointer<Ptr<Container>>(0x9DC); } }
        public Ptr x9E0_Ptr_12Bytes_DebugText { get { return ReadPointer(0x9E0); } }
        public int _x9E4 { get { return Read<int>(0x9E4); } } // 0
        public Ptr<Container<Scene>> x9E8_Scenes { get { return ReadPointer<Container<Scene>>(0x9E8); } }
        public Ptr<SceneManager> x9EC_Ptr_1288Bytes_SceneManager { get { return ReadPointer<SceneManager>(0x9EC); } }
        public Ptr<Ptr<SceneOctree>> x9F0_Ptr_Ptr_SceneOctree { get { return ReadPointer<Ptr<SceneOctree>>(0x9F0); } }
        public Ptr<X9A4> x9F4_Ptr_16Bytes { get { return ReadPointer<X9A4>(0x9F4); } }
        public Ptr<Allocator> x9F8_Ptr_416Bytes_Allocator_296x64Bytes_AnimationManager { get { return ReadPointer<Allocator>(0x9F8); } }
        public Ptr<X9AC> x9FC_Ptr_8Bytes { get { return ReadPointer<X9AC>(0x9FC); } }
        public int xA00 { get { return Read<int>(0xA00); } }
        public Ptr<int> xA04_Ptr_4Bytes_Sky { get { return ReadPointer<int>(0xA04); } }
        public Ptr<Wind> xA08_Ptr_64Bytes_Wind { get { return ReadPointer<Wind>(0xA08); } }
        public Ptr<AnimationBuffer> xA0C_Ptr_40Bytes_AnimationBuffer { get { return ReadPointer<AnimationBuffer>(0xA0C); } }
        public Ptr<Sprites> xA10_Ptr_660Bytes_Sprites { get { return ReadPointer<Sprites>(0xA10); } }
        public Ptr<SubObjectGfx> xA14_Ptr_280Bytes_SubObjectGfx { get { return ReadPointer<SubObjectGfx>(0xA14); } }
        public Ptr<RWindowMgr> xA18_Ptr_20Bytes_RWindowMgr { get { return ReadPointer<RWindowMgr>(0xA18); } }
        public Ptr<UIManager> xA1C_Ptr_10000Bytes_UI { get { return ReadPointer<UIManager>(0xA1C); } }
        public Ptr<CameraManager> xA20_Ptr_5088Bytes_CameraManager { get { return ReadPointer<CameraManager>(0xA20); } }
        public Ptr<Container<World>> xA24_CWorlds { get { return ReadPointer<Container<World>>(0xA24); } }
        public Ptr<WorldManager> xA28_Ptr_80Bytes_WorldManager { get { return ReadPointer<WorldManager>(0xA28); } }
        public Ptr<Player> xA2C_Player { get { return ReadPointer<Player>(0xA2C); } }
        public Ptr<PlayerInput> xA30_PlayerInput { get { return ReadPointer<PlayerInput>(0xA30); } }
        public Ptr<PostFX> xA34_Ptr_1064Bytes_PostFX { get { return ReadPointer<PostFX>(0xA34); } }
        public Ptr<CutsceneManager> xA38_Ptr_CutsceneManager { get { return ReadPointer<CutsceneManager>(0xA38); } }
        public int _xA3C { get { return Read<int>(0xA3C); } } // 0
        public int _xA40 { get { return Read<int>(0xA40); } } // 0
        public Ptr<ShakeManager> xA44_Ptr_ShakeManager { get { return ReadPointer<ShakeManager>(0xA44); } }
        public Ptr<Ptr<Container>> xA48_Ptr_EffectGroups { get { return ReadPointer<Ptr<Container>>(0xA48); } }
        public Ptr<Ptr<Container>> xA4C_Ptr_CComplexEffects { get { return ReadPointer<Ptr<Container>>(0xA4C); } }
        public Ptr<Allocator> xA50_Ptr_Allocator_224x1024Bytes_TexAnim { get { return ReadPointer<Allocator>(0xA50); } }
        public Ptr<ListPack<TimedEvent>> xA54_Ptr_TimedEvents { get { return ReadPointer<ListPack<TimedEvent>>(0xA54); } }
        public Ptr<ActTransitions> xA58_Ptr_8Bytes_ActTransitions { get { return ReadPointer<ActTransitions>(0xA58); } }
        public Ptr<Allocator> xA5C_Ptr_Allocator_656x1024Bytes_ActorMovement { get { return ReadPointer<Allocator>(0xA5C); } }

        public class X790 : MemoryObject
		{
			public const int SizeOf = 0x124; // 292

            // 0x1000: Rendering?
            // 0x0200: Paused?
            // 0x0010: InGame?
            public int x000_Flags { get { return Read<int>(0x000); } }
			public int _x004 { get { return Read<int>(0x004); } }
			public int _x008 { get { return Read<int>(0x008); } }
			public int _x00C { get { return Read<int>(0x00C); } }
			public int _x010 { get { return Read<int>(0x010); } }
			public int _x014 { get { return Read<int>(0x014); } }
			public int _x018 { get { return Read<int>(0x018); } }
			public int _x01C { get { return Read<int>(0x01C); } }
			public int _x020 { get { return Read<int>(0x020); } }
			public int _x024 { get { return Read<int>(0x024); } }
			public int _x028 { get { return Read<int>(0x028); } }
			public int _x02C { get { return Read<int>(0x02C); } }
			public int _x030 { get { return Read<int>(0x030); } }
			public int _x034 { get { return Read<int>(0x034); } }
			public int x038_WorldId_ { get { return Read<int>(0x038); } }
			public int _x03C { get { return Read<int>(0x03C); } }
			// 0x01: Loading?
			// 0x00: Loaded?
			public int x040_Flag { get { return Read<int>(0x040); } }
			public int _x044 { get { return Read<int>(0x044); } }
			public int _x048 { get { return Read<int>(0x048); } }
			public int _x04C { get { return Read<int>(0x04C); } }
			public int _x050 { get { return Read<int>(0x050); } }
			public int x054_FrameCount { get { return Read<int>(0x054); } }
			public int x058_FrameTimestamp { get { return Read<int>(0x058); } }
			public int _x05C { get { return Read<int>(0x05C); } }
			public int x060_Flag { get { return Read<int>(0x060); } }
			public int _x064 { get { return Read<int>(0x064); } }
			public int _x068 { get { return Read<int>(0x068); } }
			public int _x06C { get { return Read<int>(0x06C); } }
			public int _x070 { get { return Read<int>(0x070); } }
			public float x074_RiftSouls { get { return Read<int>(0x074); } }
			public int _x078 { get { return Read<int>(0x078); } }
			public int _x07C { get { return Read<int>(0x07C); } }
			public float _x080 { get { return Read<float>(0x080); } }
			public float _x084 { get { return Read<float>(0x084); } }
			public float _x088 { get { return Read<float>(0x088); } }
			public float _x08C { get { return Read<float>(0x08C); } }
			public float _x090 { get { return Read<float>(0x090); } }
			public float _x094 { get { return Read<float>(0x094); } }
			public int _x098 { get { return Read<int>(0x098); } }
			public float _x09C { get { return Read<float>(0x09C); } }
			public float _x0A0 { get { return Read<float>(0x0A0); } }
			public float _x0A4 { get { return Read<float>(0x0A4); } }
			public int _x0A8 { get { return Read<int>(0x0A8); } }
			public float _x0AC { get { return Read<float>(0x0AC); } }
			public float _x0B0 { get { return Read<float>(0x0B0); } }
			public float _x0B4 { get { return Read<float>(0x0B4); } }
			public float _x0B8 { get { return Read<float>(0x0B8); } }
			public float _x0BC { get { return Read<float>(0x0BC); } }
			public float _x0C0 { get { return Read<float>(0x0C0); } }
			public float _x0C4 { get { return Read<float>(0x0C4); } }
			public int _x0C8 { get { return Read<int>(0x0C8); } }
			public float _x0CC { get { return Read<float>(0x0CC); } }
			public int _x0D0 { get { return Read<int>(0x0D0); } }
			public int _x0D4 { get { return Read<int>(0x0D4); } }
			public int _x0D8 { get { return Read<int>(0x0D8); } }
			public int _x0DC { get { return Read<int>(0x0DC); } }
			public int _x0E0 { get { return Read<int>(0x0E0); } }
			public int _x0E4 { get { return Read<int>(0x0E4); } }
			public int _x0E8 { get { return Read<int>(0x0E8); } }
			public int _x0EC { get { return Read<int>(0x0EC); } }
			public int _x0F0 { get { return Read<int>(0x0F0); } }
			public float x0F4_RiftProgress { get { return Read<float>(0x0F4); } }
			public int _x0F8 { get { return Read<int>(0x0F8); } }
			public int _x0FC { get { return Read<int>(0x0FC); } }
			public int _x100 { get { return Read<int>(0x100); } }
			public int _x104 { get { return Read<int>(0x104); } }
			public int _x108 { get { return Read<int>(0x108); } }
			public int _x10C { get { return Read<int>(0x10C); } }
			public int _x110_IsGamePaused { get { return Read<int>(0x110); } } // 1 for Achievements, Dialogs etc ?
			public int _x114 { get { return Read<int>(0x114); } }
			public int _x118 { get { return Read<int>(0x118); } }
		}

		public class X9A4 : MemoryObject
		{
			public const int SizeOf = 0x10; // 16

			public Ptr<SphereTree> x00_Ptr_64Bytes_EnvAppTree { get { return ReadPointer<SphereTree>(0x00); } }
			public Ptr<SphereTree> x04_Ptr_64Bytes_RActorTree { get { return ReadPointer<SphereTree>(0x04); } }
			public Ptr<Map> x08_Ptr_Map { get { return ReadPointer<Map>(0x08); } }
			public Ptr<SphereTree> x0C_Ptr_64Bytes_CollisionSphere { get { return ReadPointer<SphereTree>(0x0C); } }
		}

		public class X9AC : MemoryObject
		{
			public const int SizeOf = 8;

			public Ptr<ListB> x00_Ptr_ListB { get { return ReadPointer<ListB>(0x00); } }
			public Ptr<ListB> x04_Ptr_ListB { get { return ReadPointer<ListB>(0x04); } }
		}
	}

	public partial class ObjectManager
	{
		//[ThreadStatic]
		//private static ObjectCache<ObjectManager> _instance = new ObjectCache<ObjectManager>(() =>
		//	Engine.TryGet(engine => engine.ObjectManager)) { IsFinalIfNotNull = true };

		public static ObjectManager Instance { get { return Engine.TryGet(a => a.ObjectManager); } }
	}
}
