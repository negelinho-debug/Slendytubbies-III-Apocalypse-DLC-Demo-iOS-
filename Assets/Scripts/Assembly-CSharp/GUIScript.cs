using UnityEngine;

public class GUIScript : MonoBehaviour
{
	public Animator animator;

	public Animator animator2;

	public Animator animator3;

	public Vector2 scrollPosition = Vector2.zero;

	public Vector2 scrollPos2 = Vector2.zero;

	public bool idleStandbool = true;

	public bool idleReadybool;

	public bool idleMonsterbool;

	public bool idleFightbool;

	public bool weaponStandbool;

	public bool pistolReadybool;

	public bool weaponRun;

	public bool oneHandSwordIdle;

	public bool bowIdle;

	public bool motorbikeIdle;

	public bool rollerBlade;

	public bool skateboard;

	public bool climbing;

	public bool office;

	public bool swim;

	public bool wand;

	public bool cards;

	public bool breakdance;

	public bool katana;

	public bool soccer;

	public bool giant;

	public bool zombie;

	public bool iceHockey;

	public float LHandWeight;

	public Transform LHandPos1;

	public Transform LHandPos2;

	public Transform LHandPos3;

	private void OnGUI()
	{
		scrollPos2 = GUI.BeginScrollView(new Rect(10f, 10f, 120f, 285f), scrollPos2, new Rect(0f, 0f, 100f, 525f));
		if (GUI.Button(new Rect(0f, 0f, 100f, 20f), "Idle Stand"))
		{
			IdleStand();
		}
		if (GUI.Button(new Rect(0f, 25f, 100f, 20f), "Idle Ready"))
		{
			IdleReady();
		}
		if (GUI.Button(new Rect(0f, 50f, 100f, 20f), "Idle Monster"))
		{
			IdleMonster();
		}
		if (GUI.Button(new Rect(0f, 75f, 100f, 20f), "Weapon Stand"))
		{
			WeaponStand();
		}
		if (GUI.Button(new Rect(0f, 100f, 100f, 20f), "Pistol Ready"))
		{
			PistolReady();
		}
		if (GUI.Button(new Rect(0f, 125f, 100f, 20f), "1 Hand Sword"))
		{
			OneHandSwordIdle();
		}
		if (GUI.Button(new Rect(0f, 150f, 100f, 20f), "Bow"))
		{
			BowIdle();
		}
		if (GUI.Button(new Rect(0f, 175f, 100f, 20f), "Motorbike"))
		{
			MotorbikeIdle();
		}
		if (GUI.Button(new Rect(0f, 200f, 100f, 20f), "RollerBlade"))
		{
			RollerBladeStand();
		}
		if (GUI.Button(new Rect(0f, 225f, 100f, 20f), "Skateboard"))
		{
			SkateboardIdle();
		}
		if (GUI.Button(new Rect(0f, 250f, 100f, 20f), "Climbing"))
		{
			ClimbIdle();
		}
		if (GUI.Button(new Rect(0f, 275f, 100f, 20f), "Office"))
		{
			OfficeSitting();
		}
		if (GUI.Button(new Rect(0f, 300f, 100f, 20f), "Swimming"))
		{
			Swim();
		}
		if (GUI.Button(new Rect(0f, 325f, 100f, 20f), "Wand/Staff"))
		{
			WandStand();
		}
		if (GUI.Button(new Rect(0f, 350f, 100f, 20f), "Cards"))
		{
			DealerIdle();
		}
		if (GUI.Button(new Rect(0f, 375f, 100f, 20f), "Breakdancing"))
		{
			SixStep();
		}
		if (GUI.Button(new Rect(0f, 400f, 100f, 20f), "Katana"))
		{
			KatanaNinjaDraw();
		}
		if (GUI.Button(new Rect(0f, 425f, 100f, 20f), "Soccer"))
		{
			Soccer();
		}
		if (GUI.Button(new Rect(0f, 450f, 100f, 20f), "Giant"))
		{
			Giant();
		}
		if (GUI.Button(new Rect(0f, 475f, 100f, 20f), "Zombie"))
		{
			Zombie();
		}
		if (GUI.Button(new Rect(0f, 500f, 100f, 20f), "IceHockey"))
		{
			IceHockeyIdle();
		}
		GUI.EndScrollView();
		if (idleStandbool)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 150f, 285f), scrollPosition, new Rect(0f, 0f, 120f, 2600f));
			if (GUI.Button(new Rect(0f, 0f, 100f, 20f), "Idle Cheer"))
			{
				IdleCheer();
			}
			if (GUI.Button(new Rect(0f, 25f, 120f, 20f), "Idle Come Here"))
			{
				ComeHere();
			}
			if (GUI.Button(new Rect(0f, 50f, 120f, 20f), "Idle Keep Back"))
			{
				IdleKeepBack();
			}
			if (GUI.Button(new Rect(0f, 75f, 100f, 20f), "Idle Sad"))
			{
				IdleSad();
			}
			if (GUI.Button(new Rect(0f, 100f, 100f, 20f), "Idle Walk"))
			{
				IdleWalk();
			}
			if (GUI.Button(new Rect(0f, 125f, 120f, 20f), "Idle Strafe Right"))
			{
				IdleStrafeRight();
			}
			if (GUI.Button(new Rect(0f, 150f, 120f, 20f), "Idle Strafe Left"))
			{
				IdleStrafeLeft();
			}
			if (GUI.Button(new Rect(0f, 175f, 120f, 20f), "Strafe Run Left"))
			{
				StrafeRunLeft();
			}
			if (GUI.Button(new Rect(0f, 200f, 120f, 20f), "Strafe Run Right"))
			{
				StrafeRunRight();
			}
			if (GUI.Button(new Rect(0f, 225f, 120f, 20f), "Run Backward"))
			{
				RunBackward();
			}
			if (GUI.Button(new Rect(0f, 250f, 120f, 20f), "Run Back Left"))
			{
				RunBackLeft();
			}
			if (GUI.Button(new Rect(0f, 275f, 120f, 20f), "Run Back Right"))
			{
				RunBackRight();
			}
			if (GUI.Button(new Rect(0f, 300f, 120f, 20f), "Cowboy1HandDraw"))
			{
				Cowboy1HandDraw();
			}
			if (GUI.Button(new Rect(0f, 325f, 120f, 20f), "Crate Push"))
			{
				CratePush();
			}
			if (GUI.Button(new Rect(0f, 350f, 120f, 20f), "Crate Pull"))
			{
				CratePull();
			}
			if (GUI.Button(new Rect(0f, 375f, 120f, 20f), "Idle 90 Deg Turns"))
			{
				IdleTurns();
			}
			if (GUI.Button(new Rect(0f, 400f, 120f, 20f), "Idle Meditate"))
			{
				IdleMeditate();
			}
			if (GUI.Button(new Rect(0f, 425f, 120f, 20f), "Idle 180"))
			{
				Idle180();
			}
			if (GUI.Button(new Rect(0f, 450f, 120f, 20f), "Idle Button Press"))
			{
				IdleButtonPress();
			}
			if (GUI.Button(new Rect(0f, 475f, 120f, 20f), "Idle Typing"))
			{
				IdleTyping();
			}
			if (GUI.Button(new Rect(0f, 500f, 120f, 20f), "Idle Stun"))
			{
				IdleStun();
			}
			if (GUI.Button(new Rect(0f, 525f, 120f, 20f), "Wood Cut"))
			{
				WoodCut();
			}
			if (GUI.Button(new Rect(0f, 550f, 120f, 20f), "Worker Hammer"))
			{
				WorkerHammer();
			}
			if (GUI.Button(new Rect(0f, 575f, 120f, 20f), "Worker Hammer 2"))
			{
				WorkerHammer2();
			}
			if (GUI.Button(new Rect(0f, 600f, 120f, 20f), "Worker Pickaxe"))
			{
				WorkerPickaxe();
			}
			if (GUI.Button(new Rect(0f, 625f, 120f, 20f), "Worker Pickaxe 2"))
			{
				WorkerPickaxe2();
			}
			if (GUI.Button(new Rect(0f, 650f, 120f, 20f), "Worker Shovel"))
			{
				WorkerShovel();
			}
			if (GUI.Button(new Rect(0f, 675f, 120f, 20f), "Worker Shovel 2"))
			{
				WorkerShovel2();
			}
			if (GUI.Button(new Rect(0f, 700f, 120f, 20f), "Idle Spew"))
			{
				IdleSpew();
			}
			if (GUI.Button(new Rect(0f, 725f, 120f, 20f), "Idle Mouth Wipe"))
			{
				IdleMouthWipe();
			}
			if (GUI.Button(new Rect(0f, 750f, 120f, 20f), "Crawl Idle"))
			{
				CrawlIdle();
			}
			if (GUI.Button(new Rect(0f, 775f, 120f, 20f), "Crawl Locomotion"))
			{
				CrawlLocomotion();
			}
			if (GUI.Button(new Rect(0f, 800f, 120f, 20f), "Prone Idle"))
			{
				ProneIdle();
			}
			if (GUI.Button(new Rect(0f, 825f, 120f, 20f), "Prone Locomotion"))
			{
				ProneLocomotion();
			}
			if (GUI.Button(new Rect(0f, 850f, 120f, 20f), "Idle Feed Throw"))
			{
				IdleFeedThrow();
			}
			if (GUI.Button(new Rect(0f, 875f, 120f, 20f), "Idle Standing Jump"))
			{
				IdleStandingJump();
			}
			if (GUI.Button(new Rect(0f, 900f, 120f, 20f), "Yawn"))
			{
				Yawn();
			}
			if (GUI.Button(new Rect(0f, 925f, 120f, 20f), "Heel Click"))
			{
				HeelClick();
			}
			if (GUI.Button(new Rect(0f, 950f, 120f, 20f), "Vader Choke"))
			{
				VaderChoke();
			}
			if (GUI.Button(new Rect(0f, 975f, 120f, 20f), "Watering Can Idle"))
			{
				WateringCan();
			}
			if (GUI.Button(new Rect(0f, 1000f, 120f, 20f), "Watering Can Watering"))
			{
				WateringCanWatering();
			}
			if (GUI.Button(new Rect(0f, 1025f, 120f, 20f), "Up Hill Walk"))
			{
				UpHillWalk();
			}
			if (GUI.Button(new Rect(0f, 1050f, 120f, 20f), "UphillWalk Hand Grab"))
			{
				UpHillWalkHandGrab();
			}
			if (GUI.Button(new Rect(0f, 1075f, 120f, 20f), "Walk Dehydrated"))
			{
				WalkDehydrated();
			}
			if (GUI.Button(new Rect(0f, 1100f, 120f, 20f), "Idle Sand Cover"))
			{
				IdleSandCover();
			}
			if (GUI.Button(new Rect(0f, 1125f, 120f, 20f), "Battle Roar"))
			{
				BattleRoar();
			}
			if (GUI.Button(new Rect(0f, 1150f, 120f, 20f), "Channel Cast Directed"))
			{
				ChannelCastDirected();
			}
			if (GUI.Button(new Rect(0f, 1175f, 120f, 20f), "Channel Cast Omni"))
			{
				ChannelCastOmni();
			}
			if (GUI.Button(new Rect(0f, 1200f, 120f, 20f), "Fire Breath"))
			{
				FireBreath();
			}
			if (GUI.Button(new Rect(0f, 1225f, 120f, 20f), "Mutilate"))
			{
				Mutilate();
			}
			if (GUI.Button(new Rect(0f, 1250f, 120f, 20f), "Storm Strike"))
			{
				StormStrike();
			}
			if (GUI.Button(new Rect(0f, 1275f, 120f, 20f), "Walk Backward"))
			{
				WalkBackward();
			}
			if (GUI.Button(new Rect(0f, 1300f, 120f, 20f), "Flashlight"))
			{
				Flashlight();
			}
			if (GUI.Button(new Rect(0f, 1325f, 120f, 20f), "ApplePick"))
			{
				ApplePick();
			}
			if (GUI.Button(new Rect(0f, 1350f, 120f, 20f), "Arm Flex"))
			{
				ArmFlex();
			}
			if (GUI.Button(new Rect(0f, 1375f, 120f, 20f), "Arm Flex 2"))
			{
				ArmFlex2();
			}
			if (GUI.Button(new Rect(0f, 1400f, 120f, 20f), "Arm Flex 3"))
			{
				ArmFlex3();
			}
			if (GUI.Button(new Rect(0f, 1425f, 120f, 20f), "Arm Flex 4"))
			{
				ArmFlex4();
			}
			if (GUI.Button(new Rect(0f, 1450f, 120f, 20f), "Cheer Knees"))
			{
				CheerKnees();
			}
			if (GUI.Button(new Rect(0f, 1475f, 120f, 20f), "Cheer Jump"))
			{
				CheerJump();
			}
			if (GUI.Button(new Rect(0f, 1500f, 120f, 20f), "Elvis Legs"))
			{
				ElvisLegs();
			}
			if (GUI.Button(new Rect(0f, 1525f, 120f, 20f), "Face Palm"))
			{
				FacePalm();
			}
			if (GUI.Button(new Rect(0f, 1550f, 120f, 20f), "Fishing"))
			{
				Fishing();
			}
			if (GUI.Button(new Rect(0f, 1575f, 120f, 20f), "Fist Pump"))
			{
				FistPump();
			}
			if (GUI.Button(new Rect(0f, 1600f, 120f, 20f), "Fist Pump 2"))
			{
				FistPump2();
			}
			if (GUI.Button(new Rect(0f, 1625f, 120f, 20f), "Gesture Crowd Pump"))
			{
				GestureCrowdPump();
			}
			if (GUI.Button(new Rect(0f, 1650f, 120f, 20f), "Gesture Cut Throat"))
			{
				GestureCutThroat();
			}
			if (GUI.Button(new Rect(0f, 1675f, 120f, 20f), "Gesture Hand Up"))
			{
				GestureHandUp();
			}
			if (GUI.Button(new Rect(0f, 1700f, 120f, 20f), "Gesture No Fear"))
			{
				GestureNoFear();
			}
			if (GUI.Button(new Rect(0f, 1725f, 120f, 20f), "Gesture Wonderful"))
			{
				GestureWonderful();
			}
			if (GUI.Button(new Rect(0f, 1750f, 120f, 20f), "Gesture Chest Pump Salute"))
			{
				GestureChestPumpSalute();
			}
			if (GUI.Button(new Rect(0f, 1775f, 120f, 20f), "Idle Sad Hips"))
			{
				IdleSadHips();
			}
			if (GUI.Button(new Rect(0f, 1800f, 120f, 20f), "Karate Greet"))
			{
				KarateGreet();
			}
			if (GUI.Button(new Rect(0f, 1825f, 120f, 20f), "Look Up"))
			{
				LookUp();
			}
			if (GUI.Button(new Rect(0f, 1850f, 120f, 20f), "Reveling"))
			{
				Reveling();
			}
			if (GUI.Button(new Rect(0f, 1875f, 120f, 20f), "Roar"))
			{
				Roar();
			}
			if (GUI.Button(new Rect(0f, 1900f, 120f, 20f), "Pointing"))
			{
				Pointing();
			}
			if (GUI.Button(new Rect(0f, 1925f, 120f, 20f), "Russian Dance"))
			{
				RussianDance();
			}
			if (GUI.Button(new Rect(0f, 1950f, 120f, 20f), "Running Dance"))
			{
				RunningDance();
			}
			if (GUI.Button(new Rect(0f, 1975f, 120f, 20f), "Sat Night Fever"))
			{
				SatNightFever();
			}
			if (GUI.Button(new Rect(0f, 2025f, 120f, 20f), "Walk Injured"))
			{
				WalkInjured();
			}
			if (GUI.Button(new Rect(0f, 2050f, 120f, 20f), "Knees Idle"))
			{
				KneesIdle();
			}
			if (GUI.Button(new Rect(0f, 2075f, 120f, 20f), "BackPack Off"))
			{
				BackPackOff();
			}
			if (GUI.Button(new Rect(0f, 2100f, 120f, 20f), "BackPackSearch"))
			{
				BackPackSearch();
			}
			if (GUI.Button(new Rect(0f, 2125f, 120f, 20f), "Idle Eat"))
			{
				IdleEat();
			}
			if (GUI.Button(new Rect(0f, 2150f, 120f, 20f), "Idle Drink"))
			{
				IdleDrink();
			}
			if (GUI.Button(new Rect(0f, 2175f, 120f, 20f), "IdleBandage"))
			{
				IdleBandage();
			}
			if (GUI.Button(new Rect(0f, 2200f, 120f, 20f), "Loser"))
			{
				Loser();
			}
			if (GUI.Button(new Rect(0f, 2225f, 120f, 20f), "Handstand"))
			{
				Handstand();
			}
			if (GUI.Button(new Rect(0f, 2250f, 120f, 20f), "ArmFlex5"))
			{
				ArmFlex5();
			}
			if (GUI.Button(new Rect(0f, 2275f, 120f, 20f), "ArmFlex6"))
			{
				ArmFlex6();
			}
			if (GUI.Button(new Rect(0f, 2300f, 120f, 20f), "BackPackGrab"))
			{
				BackPackGrab();
			}
			if (GUI.Button(new Rect(0f, 2325f, 120f, 20f), "Whistle"))
			{
				Whistle();
			}
			if (GUI.Button(new Rect(0f, 2350f, 120f, 20f), "Suicide Head Shot"))
			{
				SuicideHeadShot();
			}
			if (GUI.Button(new Rect(0f, 2375f, 120f, 20f), "Sexy Dance"))
			{
				SexyDance();
			}
			if (GUI.Button(new Rect(0f, 2400f, 120f, 20f), "Sexy Dance 2"))
			{
				SexyDance2();
			}
			if (GUI.Button(new Rect(0f, 2425f, 120f, 20f), "Sexy Dance 3"))
			{
				SexyDance3();
			}
			if (GUI.Button(new Rect(0f, 2450f, 120f, 20f), "Wall Sit"))
			{
				WallSit();
			}
			if (GUI.Button(new Rect(0f, 2475f, 120f, 20f), "WoodSaw"))
			{
				WoodSaw();
			}
			if (GUI.Button(new Rect(0f, 2500f, 120f, 20f), "BlackSmithHammer"))
			{
				BlackSmithHammer();
			}
			if (GUI.Button(new Rect(0f, 2525f, 120f, 20f), "BlackSmithForge"))
			{
				BlackSmithForge();
			}
			if (GUI.Button(new Rect(0f, 2550f, 120f, 20f), "Smoking 1"))
			{
				Smoking1();
			}
			if (GUI.Button(new Rect(0f, 2575f, 120f, 20f), "Smoking 2"))
			{
				Smoking2();
			}
			GUI.EndScrollView();
		}
		else if (idleReadybool)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 975f));
			if (GUI.Button(new Rect(0f, 0f, 100f, 20f), "Ready Fight") && animator.GetFloat("Curve") == 0f)
			{
				IdleFight();
			}
			if (GUI.Button(new Rect(0f, 25f, 100f, 20f), "Ready Crouch"))
			{
				IdleReadyCrouch();
			}
			if (GUI.Button(new Rect(0f, 50f, 100f, 20f), "Crouch 180"))
			{
				Crouch180();
			}
			if (GUI.Button(new Rect(0f, 75f, 100f, 20f), "Crouch Walk"))
			{
				CrouchWalk();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "Crouch Walk Backward"))
			{
				CrouchWalkBackward();
			}
			if (GUI.Button(new Rect(0f, 125f, 150f, 20f), "Crouch Strafe Left"))
			{
				CrouchStrafeLeft();
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 20f), "Crouch Strafe Right"))
			{
				CrouchStrafeRight();
			}
			if (GUI.Button(new Rect(0f, 175f, 100f, 20f), "Ready Look"))
			{
				IdleReadyLook();
			}
			if (GUI.Button(new Rect(0f, 200f, 150f, 20f), "Wizard 1 Hand Throw"))
			{
				Wizard1HandThrow();
			}
			if (GUI.Button(new Rect(0f, 225f, 150f, 20f), "Wizard 2 Hand Throw"))
			{
				Wizard2HandThrow();
			}
			if (GUI.Button(new Rect(0f, 250f, 150f, 20f), "Wizard Block"))
			{
				WizardBlock();
			}
			if (GUI.Button(new Rect(0f, 275f, 150f, 20f), "Wizard Overhead"))
			{
				WizardOverhead();
			}
			if (GUI.Button(new Rect(0f, 300f, 150f, 20f), "Wizard Power Up"))
			{
				WizardPowerUp();
			}
			if (GUI.Button(new Rect(0f, 325f, 150f, 20f), "Wizard Eye Beam"))
			{
				WizardEyeBeam();
			}
			if (GUI.Button(new Rect(0f, 350f, 150f, 20f), "Wizard Neo Block"))
			{
				WizardNeoBlock();
			}
			if (GUI.Button(new Rect(0f, 375f, 150f, 20f), "Idle Dodge Left"))
			{
				IdleDodgeLeft();
			}
			if (GUI.Button(new Rect(0f, 400f, 150f, 20f), "Idle Dodge Right"))
			{
				IdleDodgeRight();
			}
			if (GUI.Button(new Rect(0f, 425f, 150f, 20f), "Idle Die"))
			{
				IdleDie();
			}
			if (GUI.Button(new Rect(0f, 450f, 150f, 20f), "Idle Run"))
			{
				IdleRun();
			}
			if (GUI.Button(new Rect(0f, 475f, 150f, 20f), "Run Jump"))
			{
				RunJump();
			}
			if (GUI.Button(new Rect(0f, 500f, 150f, 20f), "Run Dive"))
			{
				RunDive();
			}
			if (GUI.Button(new Rect(0f, 525f, 150f, 20f), "Idle Fly"))
			{
				IdleFly();
			}
			if (GUI.Button(new Rect(0f, 550f, 150f, 20f), "Fly Forward"))
			{
				FlyForward();
			}
			if (GUI.Button(new Rect(0f, 575f, 150f, 20f), "Fly Backward"))
			{
				FlyBackward();
			}
			if (GUI.Button(new Rect(0f, 600f, 150f, 20f), "Fly Left"))
			{
				FlyLeft();
			}
			if (GUI.Button(new Rect(0f, 625f, 150f, 20f), "Fly Right"))
			{
				FlyRight();
			}
			if (GUI.Button(new Rect(0f, 650f, 150f, 20f), "Fly Up"))
			{
				FlyUp();
			}
			if (GUI.Button(new Rect(0f, 675f, 150f, 20f), "Fly Down"))
			{
				FlyDown();
			}
			if (GUI.Button(new Rect(0f, 700f, 120f, 20f), "Running Slide"))
			{
				IdleSlide();
			}
			if (GUI.Button(new Rect(0f, 725f, 120f, 20f), "Idle Die 2"))
			{
				IdleDie2();
			}
			if (GUI.Button(new Rect(0f, 750f, 120f, 20f), "Loot"))
			{
				Loot();
			}
			if (GUI.Button(new Rect(0f, 775f, 120f, 20f), "SneakIdle"))
			{
				SneakIdle();
			}
			if (GUI.Button(new Rect(0f, 800f, 120f, 20f), "SneakForward"))
			{
				SneakForward();
			}
			if (GUI.Button(new Rect(0f, 825f, 120f, 20f), "SneakBackward"))
			{
				SneakBackward();
			}
			if (GUI.Button(new Rect(0f, 850f, 120f, 20f), "SneakLeft"))
			{
				SneakLeft();
			}
			if (GUI.Button(new Rect(0f, 875f, 120f, 20f), "SneakRight"))
			{
				SneakRight();
			}
			if (GUI.Button(new Rect(0f, 900f, 120f, 20f), "Wall Run Left"))
			{
				WallRunLeft();
			}
			if (GUI.Button(new Rect(0f, 925f, 120f, 20f), "Wall Run Right"))
			{
				WallRunRight();
			}
			if (GUI.Button(new Rect(0f, 950f, 120f, 20f), "Roll"))
			{
				Roll();
			}
			GUI.EndScrollView();
		}
		else if (idleFightbool)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 110f), scrollPosition, new Rect(0f, 0f, 150f, 175f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Back to Idle Ready"))
			{
				IdleReady();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Front Kick"))
			{
				FrontKick();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Face Hit"))
			{
				FaceHit();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "L Hand Punch"))
			{
				LHandPunch();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "R Hand Punch"))
			{
				RHandPunch();
			}
			if (GUI.Button(new Rect(0f, 125f, 150f, 20f), "360 Death"))
			{
				SpinDeath();
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 20f), "RoundHouse"))
			{
				RoundHouse();
			}
			GUI.EndScrollView();
		}
		else if (weaponStandbool)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 375f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Weapon Ready"))
			{
				WeaponReady();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Weapon Instant"))
			{
				WeaponInstant();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Weapon Fire"))
			{
				WeaponFire();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Weapon Ready Fire"))
			{
				WeaponReadyFire();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "Weapon Reload"))
			{
				WeaponReload();
			}
			if (GUI.Button(new Rect(0f, 125f, 150f, 20f), "Shotgun Fire"))
			{
				ShotgunFire();
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 20f), "Shotgun Ready Fire"))
			{
				ShotgunReadyFire();
			}
			if (GUI.Button(new Rect(0f, 175f, 175f, 20f), "Shotgun Reload Chamber"))
			{
				ShotgunReloadChamber();
			}
			if (GUI.Button(new Rect(0f, 200f, 175f, 20f), "Shotgun Reload Magazine"))
			{
				ShotgunReloadMagazine();
			}
			if (GUI.Button(new Rect(0f, 225f, 150f, 20f), "Nade Throw"))
			{
				NadeThrow();
			}
			if (GUI.Button(new Rect(0f, 250f, 150f, 20f), "Weapon Run"))
			{
				WeaponRun();
			}
			if (GUI.Button(new Rect(0f, 275f, 150f, 20f), "Weapon Strafe Run Left"))
			{
				WeaponStrafeRunLeft();
			}
			if (GUI.Button(new Rect(0f, 300f, 150f, 20f), "Weapon Strafe Run Right"))
			{
				WeaponStrafeRunRight();
			}
			if (GUI.Button(new Rect(0f, 325f, 150f, 20f), "Weapon Run Backward"))
			{
				WeaponRunBackward();
			}
			if (GUI.Button(new Rect(0f, 350f, 150f, 20f), "Weapon Stab"))
			{
				WeaponStab();
			}
			GUI.EndScrollView();
		}
		else if (pistolReadybool)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 100f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Pistol Instant"))
			{
				PistolInstant();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Pistol Fire"))
			{
				PistolFire();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Pistol Reload"))
			{
				PistolReload();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Pistol Quick Stab"))
			{
				PistolLeftHandStab();
			}
			GUI.EndScrollView();
		}
		else if (oneHandSwordIdle)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 220f, 285f), scrollPosition, new Rect(0f, 0f, 200f, 525f));
			if (GUI.Button(new Rect(0f, 0f, 200f, 20f), "1 Hand Sword Ready"))
			{
				OneHandSwordReady();
			}
			if (GUI.Button(new Rect(0f, 25f, 200f, 20f), "1 Hand Sword Swing"))
			{
				OneHandSwordSwing();
			}
			if (GUI.Button(new Rect(0f, 50f, 200f, 20f), "1 Hand Sword Back Swing"))
			{
				OneHandSwordBackSwing();
			}
			if (GUI.Button(new Rect(0f, 75f, 200f, 20f), "1 Hand Sword Jab"))
			{
				OneHandSwordJab();
			}
			if (GUI.Button(new Rect(0f, 100f, 200f, 20f), "1 Hand Sword Block"))
			{
				OneHandSwordBlock();
			}
			if (GUI.Button(new Rect(0f, 125f, 200f, 20f), "1 Hand Sword Shield Bash"))
			{
				OneHandSwordShieldBash();
			}
			if (GUI.Button(new Rect(0f, 150f, 200f, 20f), "1 Hand Sword Charge Up"))
			{
				OneHandSwordChargeUp();
			}
			if (GUI.Button(new Rect(0f, 175f, 200f, 20f), "1 H Sword Charge Heavy Bash"))
			{
				OneHandSwordChargeHeavyBash();
			}
			if (GUI.Button(new Rect(0f, 200f, 200f, 20f), "1 Hand Sword Charge Swipe"))
			{
				OneHandSwordChargeSwipe();
			}
			if (GUI.Button(new Rect(0f, 225f, 200f, 20f), "1 Hand Sword Run"))
			{
				OneHandSwordRun();
			}
			if (GUI.Button(new Rect(0f, 250f, 200f, 20f), "1 Hand Sword Strafe Left"))
			{
				OneHandSwordStrafeLeft();
			}
			if (GUI.Button(new Rect(0f, 275f, 200f, 20f), "1 Hand Sword Strafe Right"))
			{
				OneHandSwordStrafeRight();
			}
			if (GUI.Button(new Rect(0f, 300f, 200f, 20f), "1 Hand Sword Roll Attack"))
			{
				OneHandSwordRollAttack();
			}
			if (GUI.Button(new Rect(0f, 325f, 200f, 20f), "1 Hand Heavy Swing"))
			{
				OneHandHeavySwing();
			}
			if (GUI.Button(new Rect(0f, 350f, 200f, 20f), "1 Hand Heavy Swing 2"))
			{
				OneHandHeavySwing2();
			}
			if (GUI.Button(new Rect(0f, 375f, 200f, 20f), "1 Hand Heavy Overhead"))
			{
				OneHandHeavyOverhead();
			}
			if (GUI.Button(new Rect(0f, 400f, 200f, 20f), "1 Hand Small Weapon Combo"))
			{
				OneHandSmallWeaponCombo();
			}
			if (GUI.Button(new Rect(0f, 425f, 200f, 20f), "1 Hand Sword Jab Combo"))
			{
				OneHandSwordJabCombo();
			}
			if (GUI.Button(new Rect(0f, 450f, 200f, 20f), "1 Hand Sword Jab Foot Push"))
			{
				OneHandSwordJabFootPush();
			}
			if (GUI.Button(new Rect(0f, 475f, 200f, 20f), "1 Hand Sword Jab ready strafe left"))
			{
				OneHSwordStrafeLeft();
			}
			if (GUI.Button(new Rect(0f, 500f, 200f, 20f), "1 Hand Sword Jab ready strafe right"))
			{
				OneHSwordStrafeRight();
			}
			GUI.EndScrollView();
		}
		else if (bowIdle)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 100f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Bow Idle"))
			{
				BowIdle();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Bow Ready"))
			{
				BowReady();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Bow Instant"))
			{
				BowInstant();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Bow Fire"))
			{
				BowFire();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "Bow Ready2"))
			{
				BowReady2();
			}
			if (GUI.Button(new Rect(0f, 125f, 150f, 20f), "Bow Instant2"))
			{
				BowInstant2();
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 20f), "Bow Fire2"))
			{
				BowFire2();
			}
			GUI.EndScrollView();
		}
		else if (motorbikeIdle)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 650f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Airwalk"))
			{
				MotorbikeAirWalk();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Backward Sitting"))
			{
				MotorbikeBackwardSitting();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Backward Sitting Cheer"))
			{
				MotorbikeBackwardSittingCheer();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Backward Stand"))
			{
				MotorbikeBackwardStand();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "Handlebar Sit"))
			{
				MotorbikeHandlebarSit();
			}
			if (GUI.Button(new Rect(0f, 125f, 150f, 20f), "Hand stand"))
			{
				MotorbikeHandstand();
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 20f), "Head stand"))
			{
				MotorbikeHeadstand();
			}
			if (GUI.Button(new Rect(0f, 175f, 150f, 20f), "Heart Attack"))
			{
				MotorbikeHeartAttack();
			}
			if (GUI.Button(new Rect(0f, 200f, 150f, 20f), "Look Back"))
			{
				MotorbikeLookBack();
			}
			if (GUI.Button(new Rect(0f, 225f, 150f, 20f), "Seat Stand"))
			{
				MotorbikeSeatStand();
			}
			if (GUI.Button(new Rect(0f, 250f, 150f, 20f), "Seat Stand Wheely"))
			{
				MotorbikeSeatStandWheely();
			}
			if (GUI.Button(new Rect(0f, 275f, 150f, 20f), "Wheely"))
			{
				MotorbikeWheely();
			}
			if (GUI.Button(new Rect(0f, 300f, 150f, 20f), "Wheely No Hands"))
			{
				MotorbikeWheelyNoHands();
			}
			if (GUI.Button(new Rect(0f, 325f, 150f, 20f), "Lasso"))
			{
				MotorbikeLasso();
			}
			if (GUI.Button(new Rect(0f, 350f, 150f, 20f), "Lasso Forward"))
			{
				MotorbikeLassoFwd();
			}
			if (GUI.Button(new Rect(0f, 375f, 150f, 20f), "Lasso Back"))
			{
				MotorbikeLassoBack();
			}
			if (GUI.Button(new Rect(0f, 400f, 150f, 20f), "Lasso Left"))
			{
				MotorbikeLassoLeft();
			}
			if (GUI.Button(new Rect(0f, 425f, 150f, 20f), "Lasso Right"))
			{
				MotorbikeLassoRight();
			}
			if (GUI.Button(new Rect(0f, 450f, 150f, 20f), "Shoot Back"))
			{
				MotorbikeShootBack();
			}
			if (GUI.Button(new Rect(0f, 475f, 150f, 20f), "Shoot Forward"))
			{
				MotorbikeShootFwd();
			}
			if (GUI.Button(new Rect(0f, 500f, 150f, 20f), "Shoot Left"))
			{
				MotorbikeShootLeft();
			}
			if (GUI.Button(new Rect(0f, 525f, 150f, 20f), "Shoot Right"))
			{
				MotorbikeShootRight();
			}
			if (GUI.Button(new Rect(0f, 550f, 150f, 20f), "Turn Left"))
			{
				MotorbikeTurnLeft();
			}
			if (GUI.Button(new Rect(0f, 575f, 150f, 20f), "Turn Right"))
			{
				MotorbikeTurnRight();
			}
			if (GUI.Button(new Rect(0f, 600f, 150f, 20f), "Special Flip"))
			{
				MotorbikeSpecialFlip();
			}
			if (GUI.Button(new Rect(0f, 625f, 150f, 20f), "Superman"))
			{
				MotorbikeSuperman();
			}
			GUI.EndScrollView();
		}
		else if (rollerBlade)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 275f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Roller Blade Roll"))
			{
				RollerBladeRoll();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Roller Blade Stop"))
			{
				RollerBladeStop();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Roller Blade Jump"))
			{
				RollerBladeJump();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Roller Crossover Right"))
			{
				RollerBladeCrossoverRight();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "Roller Crossover Left"))
			{
				RollerBladeCrossoverLeft();
			}
			if (GUI.Button(new Rect(0f, 125f, 150f, 20f), "Roller Blade Back Flip"))
			{
				RollerBladeBackFlip();
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 20f), "Roller Blade Front Flip"))
			{
				RollerBladeFrontFlip();
			}
			if (GUI.Button(new Rect(0f, 175f, 150f, 20f), "Roller Blade Skate Fwd"))
			{
				RollerBladeSkateFwd();
			}
			if (GUI.Button(new Rect(0f, 200f, 150f, 20f), "Roller Blade Turn Left"))
			{
				RollerBladeTurnLeft();
			}
			if (GUI.Button(new Rect(0f, 225f, 150f, 20f), "Roller Blade Turn Right"))
			{
				RollerBladeTurnRight();
			}
			if (GUI.Button(new Rect(0f, 250f, 150f, 20f), "Roller Blade Grind Royale"))
			{
				RollerBladeGrindRoyale();
			}
			GUI.EndScrollView();
		}
		else if (skateboard)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 50f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Skateboard Idle"))
			{
				SkateboardIdle();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "SkateboardKickPush"))
			{
				SkateboardKickPush();
			}
			GUI.EndScrollView();
		}
		else if (climbing)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 100f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Climbing Idle"))
			{
				ClimbIdle();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Climb Up"))
			{
				ClimbUp();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Climb Left"))
			{
				ClimbLeft();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Climb Right"))
			{
				ClimbRight();
			}
			GUI.EndScrollView();
		}
		else if (office)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 300f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Sitting Idle"))
			{
				OfficeSitting();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Sitting Leg Cross"))
			{
				OfficeSittingLegCross();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Sitting 45 Degress leg"))
			{
				OfficeSitting45DegLeg();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Sitting 1 Leg Straight"))
			{
				OfficeSitting1LegStraight();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "Sitting Eyes Rub"))
			{
				OfficeSittingEyesRub();
			}
			if (GUI.Button(new Rect(0f, 125f, 150f, 20f), "Sitting Hand Rest Finger Tap"))
			{
				OfficeSittingHandRestFingerTap();
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 20f), "Sitting Mouse Movement"))
			{
				OfficeSittingMouseMovement();
			}
			if (GUI.Button(new Rect(0f, 175f, 150f, 20f), "Sitting Reading"))
			{
				OfficeSittingReading();
			}
			if (GUI.Button(new Rect(0f, 200f, 150f, 20f), "Sitting Reading Lean Back"))
			{
				OfficeSittingReadingLeanBack();
			}
			if (GUI.Button(new Rect(0f, 225f, 150f, 20f), "Sitting Reading Page Flip"))
			{
				OfficeSittingReadingPageFlip();
			}
			if (GUI.Button(new Rect(0f, 250f, 150f, 20f), "Sitting Reading Coffee Sip"))
			{
				OfficeSittingReadingCoffeeSip();
			}
			if (GUI.Button(new Rect(0f, 275f, 120f, 20f), "Sewing"))
			{
				Sewing();
			}
			GUI.EndScrollView();
		}
		else if (swim)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 275f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Swim Idle"))
			{
				Swim();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Swim Freestyle"))
			{
				SwimFreestyle();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Swim Dog Paddle"))
			{
				SwimDogPaddle();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Scuba Swim"))
			{
				ScubaSwim();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "Scuba OK"))
			{
				ScubaOK();
			}
			GUI.EndScrollView();
		}
		else if (wand)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 275f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Wand Stand"))
			{
				WandStand();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Wand Attack"))
			{
				WandAttack();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Wand Attack 2"))
			{
				WandAttack2();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Staff Stand"))
			{
				StaffStand();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "Staff Attack"))
			{
				StaffAttack();
			}
			if (GUI.Button(new Rect(0f, 125f, 150f, 20f), "Staff Heal"))
			{
				StaffHeal();
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 20f), "Staff Power Up"))
			{
				StaffPowerUp();
			}
			GUI.EndScrollView();
		}
		else if (cards)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 125f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Dealer Idle"))
			{
				DealerIdle();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Dealer Shuffle"))
			{
				DealerShuffle();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Dealer Fan"))
			{
				DealerFan();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Card Player Idle"))
			{
				CardPlayerIdle();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "Card Player Look"))
			{
				CardPlayerLook();
			}
			GUI.EndScrollView();
		}
		else if (breakdance)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 125f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Six Step"))
			{
				SixStep();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "2000"))
			{
				TwoThousand();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Deadman Float"))
			{
				DeadmanFloat();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Flares"))
			{
				Flares();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "Criticals"))
			{
				Criticals();
			}
			if (GUI.Button(new Rect(0f, 125f, 150f, 20f), "Windmill"))
			{
				Windmill();
			}
			GUI.EndScrollView();
		}
		else if (katana)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 125f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Katana Ninja Draw"))
			{
				KatanaNinjaDraw();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Katana Ready"))
			{
				KatanaReady();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Katana Ready High"))
			{
				KatanaReadyHigh();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Katana Ready Low"))
			{
				KatanaReadyLow();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "Katana Vertical Swing"))
			{
				KatanaVerticalSwing();
			}
			if (GUI.Button(new Rect(0f, 125f, 150f, 20f), "Katana 45Deg Swing"))
			{
				Katana45DegSwing();
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 20f), "Katana Horizontal Swing"))
			{
				KatanaHorizontalSwing();
			}
			if (GUI.Button(new Rect(0f, 175f, 150f, 20f), "Katana Upper Block"))
			{
				KatanaUpperBlock();
			}
			GUI.EndScrollView();
		}
		else if (soccer)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 400f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Keeper Ready"))
			{
				SoccerKeeperReady();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Keeper Strafe Right"))
			{
				SoccerKeeperStrafeRight();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Keeper Strafe Left"))
			{
				SoccerKeeperStrafeLeft();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Keeper Jump"))
			{
				SoccerKeeperJump();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "Keeper Strafe Dive Far Left"))
			{
				SoccerKeeperStrafeDiveFarLeft();
			}
			if (GUI.Button(new Rect(0f, 125f, 150f, 20f), "Keeper Strafe Dive Far Right"))
			{
				SoccerKeeperStrafeDiveFarRight();
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 20f), "Keeper Strafe Dive Close Left"))
			{
				SoccerKeeperStrafeDiveCloseLeft();
			}
			if (GUI.Button(new Rect(0f, 175f, 150f, 20f), "Keeper Strafe Dive Close Right"))
			{
				SoccerKeeperStrafeDiveCloseRight();
			}
			if (GUI.Button(new Rect(0f, 200f, 150f, 20f), "Soccer Throw"))
			{
				SoccerThrow();
			}
			if (GUI.Button(new Rect(0f, 225f, 150f, 20f), "Soccer Tackle"))
			{
				SoccerTackle();
			}
			if (GUI.Button(new Rect(0f, 250f, 150f, 20f), "Soccer Walk"))
			{
				SoccerWalk();
			}
			if (GUI.Button(new Rect(0f, 275f, 150f, 20f), "Soccer Start Kick"))
			{
				SoccerStartKick();
			}
			if (GUI.Button(new Rect(0f, 300f, 150f, 20f), "Soccer Sprint"))
			{
				SoccerSprint();
			}
			if (GUI.Button(new Rect(0f, 325f, 150f, 20f), "Soccer Run"))
			{
				SoccerRun();
			}
			if (GUI.Button(new Rect(0f, 350f, 150f, 20f), "Soccer Pass Heavy"))
			{
				SoccerPassHeavy();
			}
			if (GUI.Button(new Rect(0f, 375f, 150f, 20f), "Soccer Pass Light"))
			{
				SoccerPassLight();
			}
			GUI.EndScrollView();
		}
		else if (giant)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 275f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Giant 2 Hand Slam Idle"))
			{
				Giant2HandSlamIdle();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Giant 2 Hand Slam Swing"))
			{
				Giant2HandSlamSwing();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Giant 3 Hit Combo"))
			{
				Giant3HitCombo();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Giant 3 Hit Combo 2"))
			{
				Giant3HitCombo2();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "GiantGrabIdle"))
			{
				GiantGrabIdle();
			}
			if (GUI.Button(new Rect(0f, 125f, 150f, 20f), "GiantGrabIdle2"))
			{
				GiantGrabIdle2();
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 20f), "Giant Grab Throw"))
			{
				GiantGrabThrow();
			}
			if (GUI.Button(new Rect(0f, 175f, 150f, 20f), "Giant Grab Throw 2"))
			{
				GiantGrabThrow2();
			}
			if (GUI.Button(new Rect(0f, 200f, 150f, 20f), "Giant Eat"))
			{
				GiantEat();
			}
			if (GUI.Button(new Rect(0f, 225f, 150f, 20f), "Giant 2 Hand Grab/Throw"))
			{
				Giant2HandGrab();
			}
			GUI.EndScrollView();
		}
		else if (zombie)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 125f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "Zombie Idle"))
			{
				Zombie();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "Zombie Idle 2"))
			{
				ZombieIdle2();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "Zombie Walk"))
			{
				ZombieWalk();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "Zombie Crawl"))
			{
				ZombieCrawl();
			}
			GUI.EndScrollView();
		}
		else if (iceHockey)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(150f, 10f, 180f, 285f), scrollPosition, new Rect(0f, 0f, 150f, 250f));
			if (GUI.Button(new Rect(0f, 0f, 150f, 20f), "IceHockey Idle"))
			{
				IceHockeyIdle();
			}
			if (GUI.Button(new Rect(0f, 25f, 150f, 20f), "IceHockey Deke Middle"))
			{
				IceHockeyDekeMiddle();
			}
			if (GUI.Button(new Rect(0f, 50f, 150f, 20f), "IceHockey Goalie Ready"))
			{
				IceHockeyGoalieReady();
			}
			if (GUI.Button(new Rect(0f, 75f, 150f, 20f), "IceHockey Pass Left"))
			{
				IceHockeyPassLeft();
			}
			if (GUI.Button(new Rect(0f, 100f, 150f, 20f), "IceHockey Pass Right"))
			{
				IceHockeyPassRight();
			}
			if (GUI.Button(new Rect(0f, 125f, 150f, 20f), "IceHockey Shot Left"))
			{
				IceHockeyShotLeft();
			}
			if (GUI.Button(new Rect(0f, 150f, 150f, 20f), "IceHockey Shot Right"))
			{
				IceHockeyShotRight();
			}
			if (GUI.Button(new Rect(0f, 175f, 150f, 20f), "IceHockey Goalie Save 2"))
			{
				IceHockeyGoalieSave1();
			}
			if (GUI.Button(new Rect(0f, 200f, 150f, 20f), "IceHockey Goalie Save 2"))
			{
				IceHockeyGoalieSave2();
			}
			GUI.EndScrollView();
		}
	}

	private void IceHockeyGoalieSave2()
	{
		IceHockeyGoalieReady();
		animator.SetTrigger("IceHockeyGoalieSave2");
		animator2.SetTrigger("IceHockeyGoalieSave2");
		animator3.SetTrigger("IceHockeyGoalieSave2");
	}

	private void IceHockeyGoalieSave1()
	{
		IceHockeyGoalieReady();
		animator.SetTrigger("IceHockeyGoalieSave1");
		animator2.SetTrigger("IceHockeyGoalieSave1");
		animator3.SetTrigger("IceHockeyGoalieSave1");
	}

	private void IceHockeyShotRight()
	{
		IceHockeyDekeMiddle();
		animator.SetTrigger("IceHockeyShotRight");
		animator2.SetTrigger("IceHockeyShotRight");
		animator3.SetTrigger("IceHockeyShotRight");
	}

	private void IceHockeyShotLeft()
	{
		IceHockeyDekeMiddle();
		animator.SetTrigger("IceHockeyShotLeft");
		animator2.SetTrigger("IceHockeyShotLeft");
		animator3.SetTrigger("IceHockeyShotLeft");
	}

	private void IceHockeyPassRight()
	{
		IceHockeyDekeMiddle();
		animator.SetTrigger("IceHockeyPassRight");
		animator2.SetTrigger("IceHockeyPassRight");
		animator3.SetTrigger("IceHockeyPassRight");
	}

	private void IceHockeyPassLeft()
	{
		IceHockeyDekeMiddle();
		animator.SetTrigger("IceHockeyPassLeft");
		animator2.SetTrigger("IceHockeyPassLeft");
		animator3.SetTrigger("IceHockeyPassLeft");
	}

	private void IceHockeyGoalieReady()
	{
		IceHockeyIdle();
		animator.SetBool("IceHockeyGoalieReady", true);
		animator2.SetBool("IceHockeyGoalieReady", true);
		animator3.SetBool("IceHockeyGoalieReady", true);
	}

	private void IceHockeyDekeMiddle()
	{
		IceHockeyIdle();
		animator.SetBool("IceHockeyDekeMiddle", true);
		animator2.SetBool("IceHockeyDekeMiddle", true);
		animator3.SetBool("IceHockeyDekeMiddle", true);
	}

	private void IceHockeyIdle()
	{
		Falses();
		iceHockey = true;
		animator.SetBool("IdleStand", true);
		animator2.SetBool("IdleStand", true);
		animator3.SetBool("IdleStand", true);
		animator.SetBool("IceHockeyIdle", true);
		animator2.SetBool("IceHockeyIdle", true);
		animator3.SetBool("IceHockeyIdle", true);
	}

	private void Smoking2()
	{
		IdleStand();
		animator.SetTrigger("Smoking2");
		animator2.SetTrigger("Smoking2");
		animator3.SetTrigger("Smoking2");
	}

	private void Smoking1()
	{
		IdleStand();
		animator.SetTrigger("Smoking1");
		animator2.SetTrigger("Smoking1");
		animator3.SetTrigger("Smoking1");
	}

	private void OneHSwordStrafeLeft()
	{
		OneHandSwordReady();
		animator.SetBool("1HandSwordStrafeLeft", true);
		animator2.SetBool("1HandSwordStrafeLeft", true);
		animator3.SetBool("1HandSwordStrafeLeft", true);
	}

	private void OneHSwordStrafeRight()
	{
		OneHandSwordReady();
		animator.SetBool("1HandSwordStrafeRight", true);
		animator2.SetBool("1HandSwordStrafeRight", true);
		animator3.SetBool("1HandSwordStrafeRight", true);
	}

	private void WeaponStab()
	{
		WeaponReady();
		animator.SetTrigger("WeaponStab");
		animator2.SetTrigger("WeaponStab");
		animator3.SetTrigger("WeaponStab");
	}

	private void Giant2HandGrab()
	{
		Giant();
		animator.SetTrigger("Giant2HandGrab");
		animator2.SetTrigger("Giant2HandGrab");
		animator3.SetTrigger("Giant2HandGrab");
	}

	private void ZombieCrawl()
	{
		ZombieWalk();
		animator.SetBool("ZombieCrawl", true);
		animator2.SetBool("ZombieCrawl", true);
		animator3.SetBool("ZombieCrawl", true);
	}

	private void ZombieWalk()
	{
		Zombie();
		animator.SetBool("ZombieWalk", true);
		animator2.SetBool("ZombieWalk", true);
		animator3.SetBool("ZombieWalk", true);
	}

	private void ZombieIdle2()
	{
		Zombie();
		animator.SetTrigger("ZombieIdle2");
		animator2.SetTrigger("ZombieIdle2");
		animator3.SetTrigger("ZombieIdle2");
	}

	private void Zombie()
	{
		Falses();
		zombie = true;
		animator.SetBool("IdleStand", true);
		animator2.SetBool("IdleStand", true);
		animator3.SetBool("IdleStand", true);
		animator.SetBool("ZombieIdle", true);
		animator2.SetBool("ZombieIdle", true);
		animator3.SetBool("ZombieIdle", true);
	}

	private void BlackSmithForge()
	{
		IdleStand();
		animator.SetTrigger("BlackSmithForge");
		animator2.SetTrigger("BlackSmithForge");
		animator3.SetTrigger("BlackSmithForge");
	}

	private void BlackSmithHammer()
	{
		IdleStand();
		animator.SetBool("BlackSmithHammer", true);
		animator2.SetBool("BlackSmithHammer", true);
		animator3.SetBool("BlackSmithHammer", true);
	}

	private void WoodSaw()
	{
		IdleStand();
		animator.SetBool("WoodSaw", true);
		animator2.SetBool("WoodSaw", true);
		animator3.SetBool("WoodSaw", true);
	}

	private void GiantEat()
	{
		GiantGrabIdle();
		animator.SetTrigger("GiantEat");
		animator2.SetTrigger("GiantEat");
		animator3.SetTrigger("GiantEat");
	}

	private void GiantGrabThrow2()
	{
		GiantGrabIdle2();
		animator.SetTrigger("GiantGrabThrow2");
		animator2.SetTrigger("GiantGrabThrow2");
		animator3.SetTrigger("GiantGrabThrow2");
	}

	private void GiantGrabThrow()
	{
		GiantGrabIdle2();
		animator.SetTrigger("GiantGrabThrow");
		animator2.SetTrigger("GiantGrabThrow");
		animator3.SetTrigger("GiantGrabThrow");
	}

	private void GiantGrabIdle()
	{
		Giant();
		animator.SetBool("GiantGrabIdle", true);
		animator2.SetBool("GiantGrabIdle", true);
		animator3.SetBool("GiantGrabIdle", true);
	}

	private void GiantGrabIdle2()
	{
		Giant();
		animator.SetBool("GiantGrabIdle2", true);
		animator2.SetBool("GiantGrabIdle2", true);
		animator3.SetBool("GiantGrabIdle2", true);
	}

	private void Giant3HitCombo2()
	{
		Giant();
		animator.SetTrigger("Giant3HitCombo2");
		animator2.SetTrigger("Giant3HitCombo2");
		animator3.SetTrigger("Giant3HitCombo2");
	}

	private void Giant3HitCombo()
	{
		Giant();
		animator.SetTrigger("Giant3HitCombo");
		animator2.SetTrigger("Giant3HitCombo");
		animator3.SetTrigger("Giant3HitCombo");
	}

	private void Giant2HandSlamSwing()
	{
		Giant2HandSlamIdle();
		animator.SetTrigger("Giant2HandSlamSwing");
		animator2.SetTrigger("Giant2HandSlamSwing");
		animator3.SetTrigger("Giant2HandSlamSwing");
	}

	private void Giant2HandSlamIdle()
	{
		Giant();
		animator.SetBool("Giant2HandSlamIdle", true);
		animator2.SetBool("Giant2HandSlamIdle", true);
		animator3.SetBool("Giant2HandSlamIdle", true);
	}

	private void Giant()
	{
		Falses();
		giant = true;
		animator.SetBool("IdleStand", true);
		animator2.SetBool("IdleStand", true);
		animator3.SetBool("IdleStand", true);
	}

	private void SuicideHeadShot()
	{
		IdleStand();
		animator.SetTrigger("SuicideHeadShot");
		animator2.SetTrigger("SuicideHeadShot");
		animator3.SetTrigger("SuicideHeadShot");
	}

	private void SexyDance()
	{
		IdleStand();
		animator.SetTrigger("SexyDance");
		animator2.SetTrigger("SexyDance");
		animator3.SetTrigger("SexyDance");
	}

	private void SexyDance2()
	{
		IdleStand();
		animator.SetTrigger("SexyDance2");
		animator2.SetTrigger("SexyDance2");
		animator3.SetTrigger("SexyDance2");
	}

	private void SexyDance3()
	{
		IdleStand();
		animator.SetTrigger("SexyDance3");
		animator2.SetTrigger("SexyDance3");
		animator3.SetTrigger("SexyDance3");
	}

	private void IceHockeySlapShot()
	{
		IdleStand();
		animator.SetTrigger("IceHockeySlapShot");
		animator2.SetTrigger("IceHockeySlapShot");
		animator3.SetTrigger("IceHockeySlapShot");
	}

	private void WallSit()
	{
		IdleStand();
		animator.SetBool("WallSit", true);
		animator2.SetBool("WallSit", true);
		animator3.SetBool("WallSit", true);
	}

	private void Whistle()
	{
		IdleStand();
		animator.SetTrigger("Whistle");
		animator2.SetTrigger("Whistle");
		animator3.SetTrigger("Whistle");
	}

	private void OneHandHeavySwing()
	{
		OneHandSwordReady();
		animator.SetTrigger("1HandHeavySwing");
		animator2.SetTrigger("1HandHeavySwing");
		animator3.SetTrigger("1HandHeavySwing");
	}

	private void OneHandHeavySwing2()
	{
		OneHandSwordReady();
		animator.SetTrigger("1HandHeavySwing2");
		animator2.SetTrigger("1HandHeavySwing2");
		animator3.SetTrigger("1HandHeavySwing2");
	}

	private void OneHandHeavyOverhead()
	{
		OneHandSwordReady();
		animator.SetTrigger("1HandHeavyOverhead");
		animator2.SetTrigger("1HandHeavyOverhead");
		animator3.SetTrigger("1HandHeavyOverhead");
	}

	private void OneHandSmallWeaponCombo()
	{
		OneHandSwordReady();
		animator.SetTrigger("1HandSmallWeaponCombo");
		animator2.SetTrigger("1HandSmallWeaponCombo");
		animator3.SetTrigger("1HandSmallWeaponCombo");
	}

	private void OneHandSwordJabCombo()
	{
		OneHandSwordReady();
		animator.SetTrigger("1HandSwordJabCombo");
		animator2.SetTrigger("1HandSwordJabCombo");
		animator3.SetTrigger("1HandSwordJabCombo");
	}

	private void OneHandSwordJabFootPush()
	{
		OneHandSwordReady();
		animator.SetTrigger("1HandSwordJabFootPush");
		animator2.SetTrigger("1HandSwordJabFootPush");
		animator3.SetTrigger("1HandSwordJabFootPush");
	}

	private void Loser()
	{
		IdleStand();
		animator.SetTrigger("Loser");
		animator2.SetTrigger("Loser");
		animator3.SetTrigger("Loser");
	}

	private void Handstand()
	{
		IdleStand();
		animator.SetTrigger("Handstand");
		animator2.SetTrigger("Handstand");
		animator3.SetTrigger("Handstand");
	}

	private void ArmFlex5()
	{
		IdleStand();
		animator.SetTrigger("ArmFlex5");
		animator2.SetTrigger("ArmFlex5");
		animator3.SetTrigger("ArmFlex5");
	}

	private void ArmFlex6()
	{
		IdleStand();
		animator.SetTrigger("ArmFlex6");
		animator2.SetTrigger("ArmFlex6");
		animator3.SetTrigger("ArmFlex6");
	}

	private void BackPackGrab()
	{
		IdleStand();
		animator.SetTrigger("BackPackGrab");
		animator2.SetTrigger("BackPackGrab");
		animator3.SetTrigger("BackPackGrab");
	}

	private void Roll()
	{
		IdleRun();
		animator.SetTrigger("Roll");
		animator2.SetTrigger("Roll");
		animator3.SetTrigger("Roll");
	}

	private void WallRunRight()
	{
		IdleRun();
		animator.SetBool("WallRunRight", true);
		animator2.SetBool("WallRunRight", true);
		animator3.SetBool("WallRunRight", true);
	}

	private void WallRunLeft()
	{
		IdleRun();
		animator.SetBool("WallRunLeft", true);
		animator2.SetBool("WallRunLeft", true);
		animator3.SetBool("WallRunLeft", true);
	}

	private void ScubaOK()
	{
		animator.SetTrigger("ScubaOK");
		animator2.SetTrigger("ScubaOK");
		animator3.SetTrigger("ScubaOK");
	}

	private void ScubaSwim()
	{
		Swim();
		animator.SetBool("ScubaSwim", true);
		animator2.SetBool("ScubaSwim", true);
		animator3.SetBool("ScubaSwim", true);
	}

	private void RoundHouse()
	{
		IdleFight();
		animator.SetTrigger("RoundHouse");
		animator2.SetTrigger("RoundHouse");
		animator3.SetTrigger("RoundHouse");
	}

	private void IdleEat()
	{
		IdleStand();
		animator.SetTrigger("IdleEat");
		animator2.SetTrigger("IdleEat");
		animator3.SetTrigger("IdleEat");
	}

	private void IdleDrink()
	{
		IdleStand();
		animator.SetTrigger("IdleDrink");
		animator2.SetTrigger("IdleDrink");
		animator3.SetTrigger("IdleDrink");
	}

	private void IdleBandage()
	{
		IdleStand();
		animator.SetTrigger("IdleBandage");
		animator2.SetTrigger("IdleBandage");
		animator3.SetTrigger("IdleBandage");
	}

	private void BackPackSearch()
	{
		BackPackOff();
		animator.SetTrigger("BackPackSearch");
		animator2.SetTrigger("BackPackSearch");
		animator3.SetTrigger("BackPackSearch");
	}

	private void BackPackOff()
	{
		IdleStand();
		animator.SetBool("BackPackOff", true);
		animator2.SetBool("BackPackOff", true);
		animator3.SetBool("BackPackOff", true);
	}

	private void SneakRight()
	{
		SneakIdle();
		animator.SetBool("SneakRight", true);
		animator2.SetBool("SneakRight", true);
		animator3.SetBool("SneakRight", true);
	}

	private void SneakLeft()
	{
		SneakIdle();
		animator.SetBool("SneakLeft", true);
		animator2.SetBool("SneakLeft", true);
		animator3.SetBool("SneakLeft", true);
	}

	private void SneakBackward()
	{
		SneakIdle();
		animator.SetBool("SneakBackward", true);
		animator2.SetBool("SneakBackward", true);
		animator3.SetBool("SneakBackward", true);
	}

	private void SneakForward()
	{
		SneakIdle();
		animator.SetBool("SneakForward", true);
		animator2.SetBool("SneakForward", true);
		animator3.SetBool("SneakForward", true);
	}

	private void SneakIdle()
	{
		IdleReady();
		animator.SetBool("SneakIdle", true);
		animator2.SetBool("SneakIdle", true);
		animator3.SetBool("SneakIdle", true);
	}

	private void SoccerPassHeavy()
	{
		SoccerRun();
		animator.SetTrigger("SoccerPassHeavy");
		animator2.SetTrigger("SoccerPassHeavy");
		animator3.SetTrigger("SoccerPassHeavy");
	}

	private void SoccerPassLight()
	{
		SoccerRun();
		animator.SetTrigger("SoccerPassLight");
		animator2.SetTrigger("SoccerPassLight");
		animator3.SetTrigger("SoccerPassLight");
	}

	private void SoccerRun()
	{
		Soccer();
		animator.SetBool("SoccerRun", true);
		animator2.SetBool("SoccerRun", true);
		animator3.SetBool("SoccerRun", true);
	}

	private void SoccerSprint()
	{
		Soccer();
		animator.SetBool("SoccerSprint", true);
		animator2.SetBool("SoccerSprint", true);
		animator3.SetBool("SoccerSprint", true);
	}

	private void SoccerStartKick()
	{
		Soccer();
		animator.SetTrigger("SoccerStartKick");
		animator2.SetTrigger("SoccerStartKick");
		animator3.SetTrigger("SoccerStartKick");
	}

	private void SoccerWalk()
	{
		Soccer();
		animator.SetBool("SoccerWalk", true);
		animator2.SetBool("SoccerWalk", true);
		animator3.SetBool("SoccerWalk", true);
	}

	private void SoccerTackle()
	{
		Soccer();
		animator.SetTrigger("SoccerTackle");
		animator2.SetTrigger("SoccerTackle");
		animator3.SetTrigger("SoccerTackle");
	}

	private void SoccerThrow()
	{
		Soccer();
		animator.SetTrigger("SoccerThrow");
		animator2.SetTrigger("SoccerThrow");
		animator3.SetTrigger("SoccerThrow");
	}

	private void Soccer()
	{
		Falses();
		soccer = true;
		animator.SetBool("IdleStand", true);
		animator2.SetBool("IdleStand", true);
		animator3.SetBool("IdleStand", true);
	}

	private void SoccerKeeperReady()
	{
		Soccer();
		animator.SetBool("SoccerKeeperReady", true);
		animator2.SetBool("SoccerKeeperReady", true);
		animator3.SetBool("SoccerKeeperReady", true);
	}

	private void SoccerKeeperStrafeRight()
	{
		SoccerKeeperReady();
		animator.SetTrigger("SoccerKeeperStrafeRight");
		animator2.SetTrigger("SoccerKeeperStrafeRight");
		animator3.SetTrigger("SoccerKeeperStrafeRight");
	}

	private void SoccerKeeperStrafeLeft()
	{
		SoccerKeeperReady();
		animator.SetTrigger("SoccerKeeperStrafeLeft");
		animator2.SetTrigger("SoccerKeeperStrafeLeft");
		animator3.SetTrigger("SoccerKeeperStrafeLeft");
	}

	private void SoccerKeeperJump()
	{
		SoccerKeeperReady();
		animator.SetTrigger("SoccerKeeperJump");
		animator2.SetTrigger("SoccerKeeperJump");
		animator3.SetTrigger("SoccerKeeperJump");
	}

	private void SoccerKeeperStrafeDiveFarLeft()
	{
		SoccerKeeperReady();
		animator.SetTrigger("SoccerKeeperDiveStrafeFarLeft");
		animator2.SetTrigger("SoccerKeeperDiveStrafeFarLeft");
		animator3.SetTrigger("SoccerKeeperDiveStrafeFarLeft");
	}

	private void SoccerKeeperStrafeDiveFarRight()
	{
		SoccerKeeperReady();
		animator.SetTrigger("SoccerKeeperDiveStrafeFarRight");
		animator2.SetTrigger("SoccerKeeperDiveStrafeFarRight");
		animator3.SetTrigger("SoccerKeeperDiveStrafeFarRight");
	}

	private void SoccerKeeperStrafeDiveCloseLeft()
	{
		SoccerKeeperReady();
		animator.SetTrigger("SoccerKeeperDiveStrafeCloseLeft");
		animator2.SetTrigger("SoccerKeeperDiveStrafeCloseLeft");
		animator3.SetTrigger("SoccerKeeperDiveStrafeCloseLeft");
	}

	private void SoccerKeeperStrafeDiveCloseRight()
	{
		SoccerKeeperReady();
		animator.SetTrigger("SoccerKeeperDiveStrafeCloseRight");
		animator2.SetTrigger("SoccerKeeperDiveStrafeCloseRight");
		animator3.SetTrigger("SoccerKeeperDiveStrafeCloseRight");
	}

	private void Katana45DegSwing()
	{
		animator.SetTrigger("Katana45DegSwing");
		animator2.SetTrigger("Katana45DegSwing");
		animator3.SetTrigger("Katana45DegSwing");
	}

	private void KatanaHorizontalSwing()
	{
		animator.SetTrigger("KatanaHorizontalSwing");
		animator2.SetTrigger("KatanaHorizontalSwing");
		animator3.SetTrigger("KatanaHorizontalSwing");
	}

	private void KatanaUpperBlock()
	{
		animator.SetTrigger("KatanaUpperBlock");
		animator2.SetTrigger("KatanaUpperBlock");
		animator3.SetTrigger("KatanaUpperBlock");
	}

	private void KatanaVerticalSwing()
	{
		animator.SetTrigger("KatanaVerticalSwing");
		animator2.SetTrigger("KatanaVerticalSwing");
		animator3.SetTrigger("KatanaVerticalSwing");
	}

	private void KatanaReadyLow()
	{
		katana = true;
		animator.SetBool("KatanaReadyHigh", false);
		animator2.SetBool("KatanaReadyHigh", false);
		animator3.SetBool("KatanaReadyHigh", false);
		animator.SetBool("KatanaReady", false);
		animator2.SetBool("KatanaReady", false);
		animator3.SetBool("KatanaReady", false);
		animator.SetBool("KatanaReadyLow", true);
		animator2.SetBool("KatanaReadyLow", true);
		animator3.SetBool("KatanaReadyLow", true);
	}

	private void KatanaReadyHigh()
	{
		katana = true;
		animator.SetBool("KatanaReadyHigh", true);
		animator2.SetBool("KatanaReadyHigh", true);
		animator3.SetBool("KatanaReadyHigh", true);
		animator.SetBool("KatanaReady", false);
		animator2.SetBool("KatanaReady", false);
		animator3.SetBool("KatanaReady", false);
		animator.SetBool("KatanaReadyLow", false);
		animator2.SetBool("KatanaReadyLow", false);
		animator3.SetBool("KatanaReadyLow", false);
	}

	private void KatanaReady()
	{
		katana = true;
		animator.SetBool("KatanaReadyHigh", false);
		animator2.SetBool("KatanaReadyHigh", false);
		animator3.SetBool("KatanaReadyHigh", false);
		animator.SetBool("KatanaReady", true);
		animator2.SetBool("KatanaReady", true);
		animator3.SetBool("KatanaReady", true);
		animator.SetBool("KatanaReadyLow", false);
		animator2.SetBool("KatanaReadyLow", false);
		animator3.SetBool("KatanaReadyLow", false);
	}

	private void KatanaNinjaDraw()
	{
		IdleStand();
		Falses();
		katana = true;
		animator.SetTrigger("KatanaNinjaDraw");
		animator2.SetTrigger("KatanaNinjaDraw");
		animator3.SetTrigger("KatanaNinjaDraw");
		KatanaReady();
		animator.SetBool("Katana", true);
		animator2.SetBool("Katana", true);
		animator3.SetBool("Katana", true);
	}

	private void KneesIdle()
	{
		IdleStand();
		animator.SetBool("KneesIdle", true);
		animator2.SetBool("KneesIdle", true);
		animator3.SetBool("KneesIdle", true);
	}

	private void WalkInjured()
	{
		IdleStand();
		animator.SetBool("WalkInjured", true);
		animator2.SetBool("WalkInjured", true);
		animator3.SetBool("WalkInjured", true);
	}

	private void SatNightFever()
	{
		IdleStand();
		animator.SetBool("SatNightFever", true);
		animator2.SetBool("SatNightFever", true);
		animator3.SetBool("SatNightFever", true);
	}

	private void RunningDance()
	{
		IdleStand();
		animator.SetBool("RunningDance", true);
		animator2.SetBool("RunningDance", true);
		animator3.SetBool("RunningDance", true);
	}

	private void RussianDance()
	{
		IdleStand();
		animator.SetBool("RussianDance", true);
		animator2.SetBool("RussianDance", true);
		animator3.SetBool("RussianDance", true);
	}

	private void Sewing()
	{
		OfficeSittingLegCross();
		animator.SetTrigger("Sewing");
		animator2.SetTrigger("Sewing");
		animator3.SetTrigger("Sewing");
	}

	private void Pointing()
	{
		IdleStand();
		animator.SetTrigger("Pointing");
		animator2.SetTrigger("Pointing");
		animator3.SetTrigger("Pointing");
	}

	private void Roar()
	{
		IdleStand();
		animator.SetTrigger("Roar");
		animator2.SetTrigger("Roar");
		animator3.SetTrigger("Roar");
	}

	private void Reveling()
	{
		IdleStand();
		animator.SetTrigger("Reveling");
		animator2.SetTrigger("Reveling");
		animator3.SetTrigger("Reveling");
	}

	private void LookUp()
	{
		IdleStand();
		animator.SetTrigger("LookUp");
		animator2.SetTrigger("LookUp");
		animator3.SetTrigger("LookUp");
	}

	private void KarateGreet()
	{
		IdleStand();
		animator.SetTrigger("KarateGreet");
		animator2.SetTrigger("KarateGreet");
		animator3.SetTrigger("KarateGreet");
	}

	private void IdleSadHips()
	{
		IdleStand();
		animator.SetTrigger("IdleSadHips");
		animator2.SetTrigger("IdleSadHips");
		animator3.SetTrigger("IdleSadHips");
	}

	private void GestureChestPumpSalute()
	{
		IdleStand();
		animator.SetTrigger("GestureChestPumpSalute");
		animator2.SetTrigger("GestureChestPumpSalute");
		animator3.SetTrigger("GestureChestPumpSalute");
	}

	private void GestureWonderful()
	{
		IdleStand();
		animator.SetTrigger("GestureWonderful");
		animator2.SetTrigger("GestureWonderful");
		animator3.SetTrigger("GestureWonderful");
	}

	private void GestureNoFear()
	{
		IdleStand();
		animator.SetTrigger("GestureNoFear");
		animator2.SetTrigger("GestureNoFear");
		animator3.SetTrigger("GestureNoFear");
	}

	private void GestureHandUp()
	{
		IdleStand();
		animator.SetTrigger("GestureHandUp");
		animator2.SetTrigger("GestureHandUp");
		animator3.SetTrigger("GestureHandUp");
	}

	private void GestureCutThroat()
	{
		IdleStand();
		animator.SetTrigger("GestureCutThroat");
		animator2.SetTrigger("GestureCutThroat");
		animator3.SetTrigger("GestureCutThroat");
	}

	private void GestureCrowdPump()
	{
		IdleStand();
		animator.SetTrigger("GestureCrowdPump");
		animator2.SetTrigger("GestureCrowdPump");
		animator3.SetTrigger("GestureCrowdPump");
	}

	private void FistPump2()
	{
		IdleStand();
		animator.SetTrigger("FistPump2");
		animator2.SetTrigger("FistPump2");
		animator3.SetTrigger("FistPump2");
	}

	private void FistPump()
	{
		IdleStand();
		animator.SetTrigger("FistPump");
		animator2.SetTrigger("FistPump");
		animator3.SetTrigger("FistPump");
	}

	private void Fishing()
	{
		IdleStand();
		animator.SetTrigger("Fishing");
		animator2.SetTrigger("Fishing");
		animator3.SetTrigger("Fishing");
	}

	private void FacePalm()
	{
		IdleStand();
		animator.SetTrigger("FacePalm");
		animator2.SetTrigger("FacePalm");
		animator3.SetTrigger("FacePalm");
	}

	private void ElvisLegs()
	{
		IdleStand();
		animator.SetBool("ElvisLegsLoop", true);
		animator2.SetBool("ElvisLegsLoop", true);
		animator3.SetBool("ElvisLegsLoop", true);
	}

	private void CheerJump()
	{
		IdleStand();
		animator.SetTrigger("CheerJump");
		animator2.SetTrigger("CheerJump");
		animator3.SetTrigger("CheerJump");
	}

	private void CheerKnees()
	{
		IdleStand();
		animator.SetTrigger("CheerKnees");
		animator2.SetTrigger("CheerKnees");
		animator3.SetTrigger("CheerKnees");
	}

	private void ArmFlex()
	{
		IdleStand();
		animator.SetTrigger("ArmFlex");
		animator2.SetTrigger("ArmFlex");
		animator3.SetTrigger("ArmFlex");
	}

	private void ArmFlex2()
	{
		IdleStand();
		animator.SetTrigger("ArmFlex2");
		animator2.SetTrigger("ArmFlex2");
		animator3.SetTrigger("ArmFlex2");
	}

	private void ArmFlex3()
	{
		IdleStand();
		animator.SetTrigger("ArmFlex3");
		animator2.SetTrigger("ArmFlex3");
		animator3.SetTrigger("ArmFlex3");
	}

	private void ArmFlex4()
	{
		IdleStand();
		animator.SetTrigger("ArmFlex4");
		animator2.SetTrigger("ArmFlex4");
		animator3.SetTrigger("ArmFlex4");
	}

	private void ApplePick()
	{
		IdleStand();
		animator.SetTrigger("ApplePick");
		animator2.SetTrigger("ApplePick");
		animator3.SetTrigger("ApplePick");
	}

	private void Flashlight()
	{
		IdleStand();
		animator.SetBool("Flashlight", true);
		animator2.SetBool("Flashlight", true);
		animator3.SetBool("Flashlight", true);
	}

	private void WalkBackward()
	{
		IdleStand();
		animator.SetBool("WalkBackward", true);
		animator2.SetBool("WalkBackward", true);
		animator3.SetBool("WalkBackward", true);
	}

	private void Loot()
	{
		IdleReady();
		animator.SetTrigger("Loot");
		animator2.SetTrigger("Loot");
		animator3.SetTrigger("Loot");
	}

	private void IdleDie2()
	{
		IdleReady();
		animator.SetTrigger("IdleDie2");
		animator2.SetTrigger("IdleDie2");
		animator3.SetTrigger("IdleDie2");
	}

	private void Windmill()
	{
		SixStep();
		animator.SetBool("Windmill", true);
		animator2.SetBool("Windmill", true);
		animator3.SetBool("Windmill", true);
	}

	private void Criticals()
	{
		SixStep();
		animator.SetTrigger("Criticals");
		animator2.SetTrigger("Criticals");
		animator3.SetTrigger("Criticals");
	}

	private void Flares()
	{
		SixStep();
		animator.SetBool("Flares", true);
		animator2.SetBool("Flares", true);
		animator3.SetBool("Flares", true);
	}

	private void DeadmanFloat()
	{
		SixStep();
		animator.SetBool("DeadmanFloat", true);
		animator2.SetBool("DeadmanFloat", true);
		animator3.SetBool("DeadmanFloat", true);
	}

	private void TwoThousand()
	{
		SixStep();
		animator.SetBool("2000", true);
		animator2.SetBool("2000", true);
		animator3.SetBool("2000", true);
	}

	private void SixStep()
	{
		IdleStand();
		breakdance = true;
		idleStandbool = false;
		animator.SetBool("SixStep", true);
		animator2.SetBool("SixStep", true);
		animator3.SetBool("SixStep", true);
	}

	private void StormStrike()
	{
		IdleStand();
		animator.SetTrigger("StormStrike");
		animator2.SetTrigger("StormStrike");
		animator3.SetTrigger("StormStrike");
	}

	private void Mutilate()
	{
		IdleStand();
		animator.SetTrigger("Mutilate");
		animator2.SetTrigger("Mutilate");
		animator3.SetTrigger("Mutilate");
	}

	private void FireBreath()
	{
		IdleStand();
		animator.SetTrigger("FireBreath");
		animator2.SetTrigger("FireBreath");
		animator3.SetTrigger("FireBreath");
	}

	private void ChannelCastOmni()
	{
		IdleStand();
		animator.SetBool("ChannelCastOmni", true);
		animator2.SetBool("ChannelCastOmni", true);
		animator3.SetBool("ChannelCastOmni", true);
	}

	private void ChannelCastDirected()
	{
		IdleStand();
		animator.SetBool("ChannelCastDirected", true);
		animator2.SetBool("ChannelCastDirected", true);
		animator3.SetBool("ChannelCastDirected", true);
	}

	private void BattleRoar()
	{
		IdleStand();
		animator.SetTrigger("BattleRoar");
		animator2.SetTrigger("BattleRoar");
		animator3.SetTrigger("BattleRoar");
	}

	private void BowFire2()
	{
		BowInstant2();
		animator.SetTrigger("BowFire2");
		animator2.SetTrigger("BowFire2");
		animator3.SetTrigger("BowFire2");
	}

	private void BowInstant2()
	{
		BowReady2();
		animator.SetBool("BowInstant2", true);
		animator2.SetBool("BowInstant2", true);
		animator3.SetBool("BowInstant2", true);
	}

	private void BowReady2()
	{
		BowIdle();
		animator.SetBool("BowReady2", true);
		animator2.SetBool("BowReady2", true);
		animator3.SetBool("BowReady2", true);
	}

	private void IdleSandCover()
	{
		IdleStand();
		animator.SetTrigger("IdleSandCover");
		animator2.SetTrigger("IdleSandCover");
		animator3.SetTrigger("IdleSandCover");
	}

	private void WalkDehydrated()
	{
		IdleStand();
		animator.SetBool("WalkDehydrated", true);
		animator2.SetBool("WalkDehydrated", true);
		animator3.SetBool("WalkDehydrated", true);
	}

	private void UpHillWalkHandGrab()
	{
		UpHillWalk();
		animator.SetTrigger("UpHillWalkHandGrab");
		animator2.SetTrigger("UpHillWalkHandGrab");
		animator3.SetTrigger("UpHillWalkHandGrab");
	}

	private void UpHillWalk()
	{
		IdleStand();
		animator.SetBool("UpHillWalk", true);
		animator2.SetBool("UpHillWalk", true);
		animator3.SetBool("UpHillWalk", true);
	}

	private void CardPlayerLook()
	{
		CardPlayerIdle();
		animator.SetTrigger("CardPlayerLook");
		animator2.SetTrigger("CardPlayerLook");
		animator3.SetTrigger("CardPlayerLook");
	}

	private void CardPlayerIdle()
	{
		DealerIdle();
		animator.SetBool("CardPlayerIdle", true);
		animator2.SetBool("CardPlayerIdle", true);
		animator3.SetBool("CardPlayerIdle", true);
	}

	private void DealerFan()
	{
		DealerIdle();
		animator.SetTrigger("DealerFan");
		animator2.SetTrigger("DealerFan");
		animator3.SetTrigger("DealerFan");
	}

	private void DealerShuffle()
	{
		DealerIdle();
		animator.SetTrigger("DealerShuffle");
		animator2.SetTrigger("DealerShuffle");
		animator3.SetTrigger("DealerShuffle");
	}

	private void DealerIdle()
	{
		IdleStand();
		cards = true;
		idleStandbool = false;
		animator.SetBool("DealerIdle", true);
		animator2.SetBool("DealerIdle", true);
		animator3.SetBool("DealerIdle", true);
	}

	private void StaffPowerUp()
	{
		StaffStand();
		animator.SetTrigger("StaffPowerUp");
		animator2.SetTrigger("StaffPowerUp");
		animator3.SetTrigger("StaffPowerUp");
	}

	private void StaffHeal()
	{
		StaffStand();
		animator.SetTrigger("StaffHeal");
		animator2.SetTrigger("StaffHeal");
		animator3.SetTrigger("StaffHeal");
	}

	private void StaffAttack()
	{
		StaffStand();
		animator.SetTrigger("StaffAttack");
		animator2.SetTrigger("StaffAttack");
		animator3.SetTrigger("StaffAttack");
	}

	private void StaffStand()
	{
		IdleStand();
		wand = true;
		idleStandbool = false;
		animator.SetBool("StaffStand", true);
		animator2.SetBool("StaffStand", true);
		animator3.SetBool("StaffStand", true);
	}

	private void WandAttack()
	{
		WandStand();
		animator.SetTrigger("WandAttack");
		animator2.SetTrigger("WandAttack");
		animator3.SetTrigger("WandAttack");
	}

	private void WandAttack2()
	{
		WandStand();
		animator.SetTrigger("WandAttack2");
		animator2.SetTrigger("WandAttack2");
		animator3.SetTrigger("WandAttack2");
	}

	private void WandStand()
	{
		IdleStand();
		wand = true;
		idleStandbool = false;
		animator.SetBool("WandStand", true);
		animator2.SetBool("WandStand", true);
		animator3.SetBool("WandStand", true);
	}

	private void SwimDogPaddle()
	{
		Swim();
		animator.SetBool("SwimDogPaddle", true);
		animator2.SetBool("SwimDogPaddle", true);
		animator3.SetBool("SwimDogPaddle", true);
	}

	private void SwimFreestyle()
	{
		Swim();
		animator.SetBool("SwimFreestyle", true);
		animator2.SetBool("SwimFreestyle", true);
		animator3.SetBool("SwimFreestyle", true);
	}

	private void Swim()
	{
		IdleStand();
		swim = true;
		idleStandbool = false;
		animator.SetBool("Swim", true);
		animator2.SetBool("Swim", true);
		animator3.SetBool("Swim", true);
	}

	private void WateringCanWatering()
	{
		WateringCan();
		animator.SetBool("WateringCanWatering", true);
		animator2.SetBool("WateringCanWatering", true);
		animator3.SetBool("WateringCanWatering", true);
	}

	private void WateringCan()
	{
		IdleStand();
		animator.SetBool("WateringCan", true);
		animator2.SetBool("WateringCan", true);
		animator3.SetBool("WateringCan", true);
	}

	private void OfficeSittingReadingPageFlip()
	{
		OfficeSittingReading();
		animator.SetBool("OfficeSittingReadingPageFlip", true);
		animator2.SetBool("OfficeSittingReadingPageFlip", true);
		animator3.SetBool("OfficeSittingReadingPageFlip", true);
	}

	private void OfficeSittingReadingLeanBack()
	{
		OfficeSittingReading();
		animator.SetBool("OfficeSittingReadingLeanBack", true);
		animator2.SetBool("OfficeSittingReadingLeanBack", true);
		animator3.SetBool("OfficeSittingReadingLeanBack", true);
	}

	private void OfficeSittingReading()
	{
		OfficeSitting();
		animator.SetBool("OfficeSittingReading", true);
		animator2.SetBool("OfficeSittingReading", true);
		animator3.SetBool("OfficeSittingReading", true);
	}

	private void OfficeSittingReadingCoffeeSip()
	{
		OfficeSittingReading();
		animator.SetBool("OfficeSittingReadingCoffeeSip", true);
		animator2.SetBool("OfficeSittingReadingCoffeeSip", true);
		animator3.SetBool("OfficeSittingReadingCoffeeSip", true);
	}

	private void OfficeSittingMouseMovement()
	{
		OfficeSitting();
		animator.SetBool("OfficeSittingMouseMovement", true);
		animator2.SetBool("OfficeSittingMouseMovement", true);
		animator3.SetBool("OfficeSittingMouseMovement", true);
	}

	private void OfficeSittingHandRestFingerTap()
	{
		OfficeSitting();
		animator.SetBool("OfficeSittingHandRestFingerTap", true);
		animator2.SetBool("OfficeSittingHandRestFingerTap", true);
		animator3.SetBool("OfficeSittingHandRestFingerTap", true);
	}

	private void OfficeSittingEyesRub()
	{
		OfficeSitting();
		animator.SetBool("OfficeSittingEyesRub", true);
		animator2.SetBool("OfficeSittingEyesRub", true);
		animator3.SetBool("OfficeSittingEyesRub", true);
	}

	private void OfficeSitting1LegStraight()
	{
		OfficeSitting();
		animator.SetBool("OfficeSitting1LegStraight", true);
		animator2.SetBool("OfficeSitting1LegStraight", true);
		animator3.SetBool("OfficeSitting1LegStraight", true);
	}

	private void OfficeSitting45DegLeg()
	{
		OfficeSitting();
		animator.SetBool("OfficeSitting45DegLeg", true);
		animator2.SetBool("OfficeSitting45DegLeg", true);
		animator3.SetBool("OfficeSitting45DegLeg", true);
	}

	private void OfficeSittingBack()
	{
		OfficeSitting();
		animator.SetBool("OfficeSittingBack", true);
		animator2.SetBool("OfficeSittingBack", true);
		animator3.SetBool("OfficeSittingBack", true);
	}

	private void OfficeSittingLegCross()
	{
		OfficeSitting();
		animator.SetBool("OfficeSittingLegCross", true);
		animator2.SetBool("OfficeSittingLegCross", true);
		animator3.SetBool("OfficeSittingLegCross", true);
	}

	private void OfficeSitting()
	{
		IdleStand();
		office = true;
		idleStandbool = false;
		animator.SetBool("OfficeSitting", true);
		animator2.SetBool("OfficeSitting", true);
		animator3.SetBool("OfficeSitting", true);
	}

	private void Yawn()
	{
		IdleStand();
		animator.SetBool("Yawn", true);
		animator2.SetBool("Yawn", true);
		animator3.SetBool("Yawn", true);
	}

	private void HeelClick()
	{
		IdleStand();
		animator.SetBool("HeelClick", true);
		animator2.SetBool("HeelClick", true);
		animator3.SetBool("HeelClick", true);
	}

	private void VaderChoke()
	{
		IdleStand();
		animator.SetBool("VaderChoke", true);
		animator2.SetBool("VaderChoke", true);
		animator3.SetBool("VaderChoke", true);
	}

	private void SpinDeath()
	{
		IdleFight();
		animator.SetBool("360SpinDeath", true);
		animator2.SetBool("360SpinDeath", true);
		animator3.SetBool("360SpinDeath", true);
	}

	private void ClimbRight()
	{
		ClimbIdle();
		animator.SetBool("ClimbRight", true);
		animator2.SetBool("ClimbRight", true);
		animator3.SetBool("ClimbRight", true);
	}

	private void ClimbLeft()
	{
		ClimbIdle();
		animator.SetBool("ClimbLeft", true);
		animator2.SetBool("ClimbLeft", true);
		animator3.SetBool("ClimbLeft", true);
	}

	private void ClimbUp()
	{
		ClimbIdle();
		animator.SetBool("ClimbUp", true);
		animator2.SetBool("ClimbUp", true);
		animator3.SetBool("ClimbUp", true);
	}

	private void ClimbIdle()
	{
		IdleStand();
		climbing = true;
		idleStandbool = false;
		animator.SetBool("ClimbIdle", true);
		animator2.SetBool("ClimbIdle", true);
		animator3.SetBool("ClimbIdle", true);
	}

	private void SkateboardKickPush()
	{
		SkateboardIdle();
		animator.SetBool("SkateboardKickPush", true);
		animator2.SetBool("SkateboardKickPush", true);
		animator3.SetBool("SkateboardKickPush", true);
	}

	private void SkateboardIdle()
	{
		IdleStand();
		skateboard = true;
		idleStandbool = false;
		animator.SetBool("SkateboardIdle", true);
		animator2.SetBool("SkateboardIdle", true);
		animator3.SetBool("SkateboardIdle", true);
	}

	private void IdleStandingJump()
	{
		IdleStand();
		animator.SetBool("IdleStandingJump", true);
		animator2.SetBool("IdleStandingJump", true);
		animator3.SetBool("IdleStandingJump", true);
	}

	private void IdleFeedThrow()
	{
		IdleStand();
		animator.SetBool("IdleFeedThrow", true);
		animator2.SetBool("IdleFeedThrow", true);
		animator3.SetBool("IdleFeedThrow", true);
	}

	private void IdleSlide()
	{
		IdleRun();
		animator.SetBool("IdleSlide", true);
		animator2.SetBool("IdleSlide", true);
		animator3.SetBool("IdleSlide", true);
	}

	private void RollerBladeTurnRight()
	{
		RollerBladeRoll();
		animator.SetBool("RollerBladeTurnRight", true);
		animator2.SetBool("RollerBladeTurnRight", true);
		animator3.SetBool("RollerBladeTurnRight", true);
	}

	private void RollerBladeTurnLeft()
	{
		RollerBladeRoll();
		animator.SetBool("RollerBladeTurnLeft", true);
		animator2.SetBool("RollerBladeTurnLeft", true);
		animator3.SetBool("RollerBladeTurnLeft", true);
	}

	private void RollerBladeCrossoverLeft()
	{
		RollerBladeRoll();
		animator.SetBool("RollerBladeCrossoverLeft", true);
		animator2.SetBool("RollerBladeCrossoverLeft", true);
		animator3.SetBool("RollerBladeCrossoverLeft", true);
	}

	private void RollerBladeSkateFwd()
	{
		RollerBladeRoll();
		animator.SetBool("RollerBladeSkateFwd", true);
		animator2.SetBool("RollerBladeSkateFwd", true);
		animator3.SetBool("RollerBladeSkateFwd", true);
	}

	private void RollerBladeFrontFlip()
	{
		RollerBladeRoll();
		animator.SetBool("RollerBladeFrontFlip", true);
		animator2.SetBool("RollerBladeFrontFlip", true);
		animator3.SetBool("RollerBladeFrontFlip", true);
	}

	private void RollerBladeBackFlip()
	{
		RollerBladeRoll();
		animator.SetBool("RollerBladeBackFlip", true);
		animator2.SetBool("RollerBladeBackFlip", true);
		animator3.SetBool("RollerBladeBackFlip", true);
	}

	private void RollerBladeCrossoverRight()
	{
		RollerBladeRoll();
		animator.SetBool("RollerBladeCrossoverRight", true);
		animator2.SetBool("RollerBladeCrossoverRight", true);
		animator3.SetBool("RollerBladeCrossoverRight", true);
	}

	private void RollerBladeGrindRoyale()
	{
		RollerBladeJump();
		animator.SetBool("RollerBladeGrindRoyale", true);
		animator2.SetBool("RollerBladeGrindRoyale", true);
		animator3.SetBool("RollerBladeGrindRoyale", true);
	}

	private void RollerBladeJump()
	{
		RollerBladeRoll();
		animator.SetBool("RollerBladeJump", true);
		animator2.SetBool("RollerBladeJump", true);
		animator3.SetBool("RollerBladeJump", true);
	}

	private void RollerBladeStop()
	{
		RollerBladeStand();
		animator.SetBool("RollerBladeStop", true);
		animator2.SetBool("RollerBladeStop", true);
		animator3.SetBool("RollerBladeStop", true);
	}

	private void RollerBladeRoll()
	{
		RollerBladeStand();
		animator.SetBool("RollerBladeRoll", true);
		animator2.SetBool("RollerBladeRoll", true);
		animator3.SetBool("RollerBladeRoll", true);
	}

	private void RollerBladeStand()
	{
		Falses();
		IdleStand();
		idleStandbool = false;
		rollerBlade = true;
		animator.SetBool("RollerBladeStand", true);
		animator2.SetBool("RollerBladeStand", true);
		animator3.SetBool("RollerBladeStand", true);
	}

	private void MotorbikeSuperman()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeSuperman", true);
		animator2.SetBool("MotorbikeSuperman", true);
		animator3.SetBool("MotorbikeSuperman", true);
	}

	private void MotorbikeSpecialFlip()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeSpecialFlip", true);
		animator2.SetBool("MotorbikeSpecialFlip", true);
		animator3.SetBool("MotorbikeSpecialFlip", true);
	}

	private void PistolLeftHandStab()
	{
		PistolReady();
		animator.SetBool("PistolLeftHandStab", true);
		animator2.SetBool("PistolLeftHandStab", true);
		animator3.SetBool("PistolLeftHandStab", true);
	}

	private void CrouchStrafeRight()
	{
		IdleReadyCrouch();
		animator.SetBool("CrouchStrafeRight", true);
		animator2.SetBool("CrouchStrafeRight", true);
		animator3.SetBool("CrouchStrafeRight", true);
	}

	private void CrouchStrafeLeft()
	{
		IdleReadyCrouch();
		animator.SetBool("CrouchStrafeLeft", true);
		animator2.SetBool("CrouchStrafeLeft", true);
		animator3.SetBool("CrouchStrafeLeft", true);
	}

	private void CrouchWalkBackward()
	{
		IdleReadyCrouch();
		animator.SetBool("CrouchWalkBackward", true);
		animator2.SetBool("CrouchWalkBackward", true);
		animator3.SetBool("CrouchWalkBackward", true);
	}

	private void ProneLocomotion()
	{
		ProneIdle();
		animator.SetBool("ProneLocomotion", true);
		animator2.SetBool("ProneLocomotion", true);
		animator3.SetBool("ProneLocomotion", true);
	}

	private void ProneIdle()
	{
		IdleStand();
		animator.SetBool("ProneIdle", true);
		animator2.SetBool("ProneIdle", true);
		animator3.SetBool("ProneIdle", true);
	}

	private void CrawlLocomotion()
	{
		CrawlIdle();
		animator.SetBool("CrawlLocomotion", true);
		animator2.SetBool("CrawlLocomotion", true);
		animator3.SetBool("CrawlLocomotion", true);
	}

	private void CrawlIdle()
	{
		IdleStand();
		animator.SetBool("CrawlIdle", true);
		animator2.SetBool("CrawlIdle", true);
		animator3.SetBool("CrawlIdle", true);
	}

	private void IdleMouthWipe()
	{
		IdleStand();
		animator.SetBool("IdleMouthWipe", true);
		animator2.SetBool("IdleMouthWipe", true);
		animator3.SetBool("IdleMouthWipe", true);
	}

	private void IdleSpew()
	{
		IdleStand();
		animator.SetBool("IdleSpew", true);
		animator2.SetBool("IdleSpew", true);
		animator3.SetBool("IdleSpew", true);
	}

	private void RunBackRight()
	{
		RunBackward();
		animator.SetBool("RunBackRight", true);
		animator2.SetBool("RunBackRight", true);
		animator3.SetBool("RunBackRight", true);
	}

	private void RunBackLeft()
	{
		RunBackward();
		animator.SetBool("RunBackLeft", true);
		animator2.SetBool("RunBackLeft", true);
		animator3.SetBool("RunBackLeft", true);
	}

	private void WorkerShovel2()
	{
		IdleStand();
		animator.SetBool("WorkerShovel2", true);
		animator2.SetBool("WorkerShovel2", true);
		animator3.SetBool("WorkerShovel2", true);
	}

	private void WorkerShovel()
	{
		IdleStand();
		animator.SetBool("WorkerShovel", true);
		animator2.SetBool("WorkerShovel", true);
		animator3.SetBool("WorkerShovel", true);
	}

	private void WorkerPickaxe2()
	{
		IdleStand();
		animator.SetBool("WorkerPickaxe2", true);
		animator2.SetBool("WorkerPickaxe2", true);
		animator3.SetBool("WorkerPickaxe2", true);
	}

	private void WorkerPickaxe()
	{
		IdleStand();
		animator.SetBool("WorkerPickaxe", true);
		animator2.SetBool("WorkerPickaxe", true);
		animator3.SetBool("WorkerPickaxe", true);
	}

	private void WorkerHammer2()
	{
		IdleStand();
		animator.SetBool("WorkerHammer2", true);
		animator2.SetBool("WorkerHammer2", true);
		animator3.SetBool("WorkerHammer2", true);
	}

	private void WorkerHammer()
	{
		IdleStand();
		animator.SetBool("WorkerHammer", true);
		animator2.SetBool("WorkerHammer", true);
		animator3.SetBool("WorkerHammer", true);
	}

	private void WoodCut()
	{
		IdleStand();
		animator.SetBool("WoodCut", true);
		animator2.SetBool("WoodCut", true);
		animator3.SetBool("WoodCut", true);
	}

	private void OneHandSwordRollAttack()
	{
		OneHandSwordReady();
		animator.SetBool("1HandSwordRollAttack", true);
		animator2.SetBool("1HandSwordRollAttack", true);
		animator3.SetBool("1HandSwordRollAttack", true);
	}

	private void MotorbikeTurnRight()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeTurnRight", true);
		animator2.SetBool("MotorbikeTurnRight", true);
		animator3.SetBool("MotorbikeTurnRight", true);
	}

	private void MotorbikeTurnLeft()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeTurnLeft", true);
		animator2.SetBool("MotorbikeTurnLeft", true);
		animator3.SetBool("MotorbikeTurnLeft", true);
	}

	private void MotorbikeShootRight()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeShootRight", true);
		animator2.SetBool("MotorbikeShootRight", true);
		animator3.SetBool("MotorbikeShootRight", true);
	}

	private void MotorbikeShootLeft()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeShootLeft", true);
		animator2.SetBool("MotorbikeShootLeft", true);
		animator3.SetBool("MotorbikeShootLeft", true);
	}

	private void MotorbikeShootFwd()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeShootFwd", true);
		animator2.SetBool("MotorbikeShootFwd", true);
		animator3.SetBool("MotorbikeShootFwd", true);
	}

	private void MotorbikeShootBack()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeShootBack", true);
		animator2.SetBool("MotorbikeShootBack", true);
		animator3.SetBool("MotorbikeShootBack", true);
	}

	private void MotorbikeLassoRight()
	{
		MotorbikeLasso();
		animator.SetBool("MotorbikeLassoRight", true);
		animator2.SetBool("MotorbikeLassoRight", true);
		animator3.SetBool("MotorbikeLassoRight", true);
	}

	private void MotorbikeLassoLeft()
	{
		MotorbikeLasso();
		animator.SetBool("MotorbikeLassoLeft", true);
		animator2.SetBool("MotorbikeLassoLeft", true);
		animator3.SetBool("MotorbikeLassoLeft", true);
	}

	private void MotorbikeLassoBack()
	{
		MotorbikeLasso();
		animator.SetBool("MotorbikeLassoBack", true);
		animator2.SetBool("MotorbikeLassoBack", true);
		animator3.SetBool("MotorbikeLassoBack", true);
	}

	private void MotorbikeLassoFwd()
	{
		MotorbikeLasso();
		animator.SetBool("MotorbikeLassoFwd", true);
		animator2.SetBool("MotorbikeLassoFwd", true);
		animator3.SetBool("MotorbikeLassoFwd", true);
	}

	private void MotorbikeLasso()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeLasso", true);
		animator2.SetBool("MotorbikeLasso", true);
		animator3.SetBool("MotorbikeLasso", true);
	}

	private void MotorbikeWheelyNoHands()
	{
		MotorbikeWheely();
		animator.SetBool("MotorbikeWheelyNoHands", true);
		animator2.SetBool("MotorbikeWheelyNoHands", true);
		animator3.SetBool("MotorbikeWheelyNoHands", true);
	}

	private void MotorbikeWheely()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeWheely", true);
		animator2.SetBool("MotorbikeWheely", true);
		animator3.SetBool("MotorbikeWheely", true);
	}

	private void MotorbikeSeatStandWheely()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeSeatStandWheely", true);
		animator2.SetBool("MotorbikeSeatStandWheely", true);
		animator3.SetBool("MotorbikeSeatStandWheely", true);
	}

	private void MotorbikeSeatStand()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeSeatStand", true);
		animator2.SetBool("MotorbikeSeatStand", true);
		animator3.SetBool("MotorbikeSeatStand", true);
	}

	private void MotorbikeLookBack()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeLookBack", true);
		animator2.SetBool("MotorbikeLookBack", true);
		animator3.SetBool("MotorbikeLookBack", true);
	}

	private void MotorbikeHeartAttack()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeHeartAttack", true);
		animator2.SetBool("MotorbikeHeartAttack", true);
		animator3.SetBool("MotorbikeHeartAttack", true);
	}

	private void MotorbikeHeadstand()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeHeadstand", true);
		animator2.SetBool("MotorbikeHeadstand", true);
		animator3.SetBool("MotorbikeHeadstand", true);
	}

	private void MotorbikeHandstand()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeHandstand", true);
		animator2.SetBool("MotorbikeHandstand", true);
		animator3.SetBool("MotorbikeHandstand", true);
	}

	private void MotorbikeHandlebarSit()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeHandlebarSit", true);
		animator2.SetBool("MotorbikeHandlebarSit", true);
		animator3.SetBool("MotorbikeHandlebarSit", true);
	}

	private void MotorbikeBackwardStand()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeBackwardStand", true);
		animator2.SetBool("MotorbikeBackwardStand", true);
		animator3.SetBool("MotorbikeBackwardStand", true);
	}

	private void MotorbikeBackwardSittingCheer()
	{
		MotorbikeBackwardSitting();
		animator.SetBool("MotorbikeBackwardSittingCheer", true);
		animator2.SetBool("MotorbikeBackwardSittingCheer", true);
		animator3.SetBool("MotorbikeBackwardSittingCheer", true);
	}

	private void MotorbikeBackwardSitting()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeBackwardSitting", true);
		animator2.SetBool("MotorbikeBackwardSitting", true);
		animator3.SetBool("MotorbikeBackwardSitting", true);
	}

	private void MotorbikeAirWalk()
	{
		MotorbikeIdle();
		animator.SetBool("MotorbikeAirWalk", true);
		animator2.SetBool("MotorbikeAirWalk", true);
		animator3.SetBool("MotorbikeAirWalk", true);
	}

	private void MotorbikeIdle()
	{
		Falses();
		IdleStand();
		idleStandbool = false;
		motorbikeIdle = true;
		animator.SetBool("MotorbikeIdle", true);
		animator2.SetBool("MotorbikeIdle", true);
		animator3.SetBool("MotorbikeIdle", true);
	}

	private void WeaponReadyFire()
	{
		WeaponReady();
		animator.SetBool("WeaponReadyFire", true);
		animator2.SetBool("WeaponReadyFire", true);
		animator3.SetBool("WeaponReadyFire", true);
	}

	private void ShotgunReadyFire()
	{
		WeaponReady();
		animator.SetBool("ShotgunReadyFire", true);
		animator2.SetBool("ShotgunReadyFire", true);
		animator3.SetBool("ShotgunReadyFire", true);
	}

	private void IdleStun()
	{
		IdleStand();
		animator.SetBool("IdleStun", true);
		animator2.SetBool("IdleStun", true);
		animator3.SetBool("IdleStun", true);
	}

	private void OneHandSwordChargeSwipe()
	{
		OneHandSwordChargeUp();
		animator.SetBool("1HandSwordChargeSwipe", true);
		animator2.SetBool("1HandSwordChargeSwipe", true);
		animator3.SetBool("1HandSwordChargeSwipe", true);
	}

	private void OneHandSwordChargeHeavyBash()
	{
		OneHandSwordChargeUp();
		animator.SetBool("1HandSwordChargeHeavyBash", true);
		animator2.SetBool("1HandSwordChargeHeavyBash", true);
		animator3.SetBool("1HandSwordChargeHeavyBash", true);
	}

	private void OneHandSwordChargeUp()
	{
		OneHandSwordReady();
		animator.SetBool("1HandSwordChargeUp", true);
		animator2.SetBool("1HandSwordChargeUp", true);
		animator3.SetBool("1HandSwordChargeUp", true);
	}

	private void WeaponRunBackward()
	{
		WeaponReady();
		animator.SetBool("WeaponRunBackward", true);
		animator2.SetBool("WeaponRunBackward", true);
		animator3.SetBool("WeaponRunBackward", true);
	}

	private void RunBackward()
	{
		IdleStand();
		animator.SetBool("RunBackward", true);
		animator2.SetBool("RunBackward", true);
		animator3.SetBool("RunBackward", true);
	}

	private void OneHandSwordShieldBash()
	{
		OneHandSwordReady();
		animator.SetBool("1HandSwordShieldBash", true);
		animator2.SetBool("1HandSwordShieldBash", true);
		animator3.SetBool("1HandSwordShieldBash", true);
	}

	private void OneHandSwordStrafeLeft()
	{
		OneHandSwordReady();
		animator.SetBool("1HSwordStrafeRunLeft", true);
		animator2.SetBool("1HSwordStrafeRunLeft", true);
		animator3.SetBool("1HSwordStrafeRunLeft", true);
	}

	private void OneHandSwordStrafeRight()
	{
		OneHandSwordReady();
		animator.SetBool("1HSwordStrafeRunRight", true);
		animator2.SetBool("1HSwordStrafeRunRight", true);
		animator3.SetBool("1HSwordStrafeRunRight", true);
	}

	private void WeaponStrafeRunLeft()
	{
		WeaponReady();
		animator.SetBool("WeaponStrafeRunLeft", true);
		animator2.SetBool("WeaponStrafeRunLeft", true);
		animator3.SetBool("WeaponStrafeRunLeft", true);
	}

	private void WeaponStrafeRunRight()
	{
		WeaponReady();
		animator.SetBool("WeaponStrafeRunRight", true);
		animator2.SetBool("WeaponStrafeRunRight", true);
		animator3.SetBool("WeaponStrafeRunRight", true);
	}

	private void StrafeRunLeft()
	{
		IdleStand();
		animator.SetBool("StrafeRunLeft", true);
		animator2.SetBool("StrafeRunLeft", true);
		animator3.SetBool("StrafeRunLeft", true);
	}

	private void StrafeRunRight()
	{
		IdleStand();
		animator.SetBool("StrafeRunRight", true);
		animator2.SetBool("StrafeRunRight", true);
		animator3.SetBool("StrafeRunRight", true);
	}

	private void IdleTyping()
	{
		IdleStand();
		animator.SetBool("IdleTyping", true);
		animator2.SetBool("IdleTyping", true);
		animator3.SetBool("IdleTyping", true);
	}

	private void IdleButtonPress()
	{
		IdleStand();
		animator.SetBool("IdleButtonPress", true);
		animator2.SetBool("IdleButtonPress", true);
		animator3.SetBool("IdleButtonPress", true);
	}

	private void Idle180()
	{
		IdleStand();
		animator.SetBool("Idle180", true);
		animator2.SetBool("Idle180", true);
		animator3.SetBool("Idle180", true);
	}

	private void CrouchWalk()
	{
		IdleReadyCrouch();
		animator.SetBool("CrouchWalk", true);
		animator2.SetBool("CrouchWalk", true);
		animator3.SetBool("CrouchWalk", true);
	}

	private void Crouch180()
	{
		IdleReadyCrouch();
		animator.SetBool("Crouch180", true);
		animator2.SetBool("Crouch180", true);
		animator3.SetBool("Crouch180", true);
	}

	private void FlyDown()
	{
		IdleFly();
		animator.SetBool("FlyDown", true);
		animator2.SetBool("FlyDown", true);
		animator3.SetBool("FlyDown", true);
	}

	private void FlyUp()
	{
		IdleFly();
		animator.SetBool("FlyUp", true);
		animator2.SetBool("FlyUp", true);
		animator3.SetBool("FlyUp", true);
	}

	private void FlyRight()
	{
		IdleFly();
		animator.SetBool("FlyRight", true);
		animator2.SetBool("FlyRight", true);
		animator3.SetBool("FlyRight", true);
	}

	private void FlyLeft()
	{
		IdleFly();
		animator.SetBool("FlyLeft", true);
		animator2.SetBool("FlyLeft", true);
		animator3.SetBool("FlyLeft", true);
	}

	private void FlyBackward()
	{
		IdleFly();
		animator.SetBool("FlyBackward", true);
		animator2.SetBool("FlyBackward", true);
		animator3.SetBool("FlyBackward", true);
	}

	private void FlyForward()
	{
		IdleFly();
		animator.SetBool("FlyForward", true);
		animator2.SetBool("FlyForward", true);
		animator3.SetBool("FlyForward", true);
	}

	private void IdleFly()
	{
		IdleReady();
		animator.SetBool("IdleFly", true);
		animator2.SetBool("IdleFly", true);
		animator3.SetBool("IdleFly", true);
	}

	private void WizardNeoBlock()
	{
		IdleReady();
		animator.SetBool("WizardNeoBlock", true);
		animator2.SetBool("WizardNeoBlock", true);
		animator3.SetBool("WizardNeoBlock", true);
	}

	private void WizardEyeBeam()
	{
		IdleReady();
		animator.SetBool("WizardEyeBeam", true);
		animator2.SetBool("WizardEyeBeam", true);
		animator3.SetBool("WizardEyeBeam", true);
	}

	private void IdleMeditate()
	{
		IdleStand();
		animator.SetBool("IdleMeditate", true);
		animator2.SetBool("IdleMeditate", true);
		animator3.SetBool("IdleMeditate", true);
	}

	private void IdleDodgeLeft()
	{
		IdleReady();
		animator.SetBool("IdleDodgeLeft", true);
		animator2.SetBool("IdleDodgeLeft", true);
		animator3.SetBool("IdleDodgeLeft", true);
	}

	private void IdleDodgeRight()
	{
		IdleReady();
		animator.SetBool("IdleDodgeRight", true);
		animator2.SetBool("IdleDodgeRight", true);
		animator3.SetBool("IdleDodgeRight", true);
	}

	private void RunDive()
	{
		IdleRun();
		animator.SetBool("RunDive", true);
		animator2.SetBool("RunDive", true);
		animator3.SetBool("RunDive", true);
	}

	private void RunJump()
	{
		IdleRun();
		animator.SetBool("RunJump", true);
		animator2.SetBool("RunJump", true);
		animator3.SetBool("RunJump", true);
	}

	private void Cowboy1HandDraw()
	{
		IdleStand();
		animator.SetBool("Cowboy1HandDraw", true);
		animator2.SetBool("Cowboy1HandDraw", true);
		animator3.SetBool("Cowboy1HandDraw", true);
	}

	private void BowReady()
	{
		BowIdle();
		animator.SetBool("BowReady", true);
		animator2.SetBool("BowReady", true);
		animator3.SetBool("BowReady", true);
	}

	private void BowInstant()
	{
		BowReady();
		animator.SetBool("BowInstant", true);
		animator2.SetBool("BowInstant", true);
		animator3.SetBool("BowInstant", true);
	}

	private void BowFire()
	{
		BowInstant();
		animator.SetBool("BowFire", true);
		animator2.SetBool("BowFire", true);
		animator3.SetBool("BowFire", true);
	}

	private void BowIdle()
	{
		Falses();
		bowIdle = true;
		animator.SetBool("BowIdle", true);
		animator2.SetBool("BowIdle", true);
		animator3.SetBool("BowIdle", true);
	}

	private void OneHandSwordRun()
	{
		Falses();
		OneHandSwordReady();
		animator.SetBool("OneHandSwordRun", true);
		animator2.SetBool("OneHandSwordRun", true);
		animator3.SetBool("OneHandSwordRun", true);
	}

	private void OneHandSwordBlock()
	{
		OneHandSwordReady();
		animator.SetBool("OneHandSwordBlock", true);
		animator2.SetBool("OneHandSwordBlock", true);
		animator3.SetBool("OneHandSwordBlock", true);
	}

	private void OneHandSwordJab()
	{
		OneHandSwordReady();
		animator.SetBool("OneHandSwordJab", true);
		animator2.SetBool("OneHandSwordJab", true);
		animator3.SetBool("OneHandSwordJab", true);
	}

	private void OneHandSwordBackSwing()
	{
		OneHandSwordReady();
		animator.SetBool("OneHandSwordBackSwing", true);
		animator2.SetBool("OneHandSwordBackSwing", true);
		animator3.SetBool("OneHandSwordBackSwing", true);
	}

	private void OneHandSwordSwing()
	{
		OneHandSwordReady();
		animator.SetBool("OneHandSwordSwing", true);
		animator2.SetBool("OneHandSwordSwing", true);
		animator3.SetBool("OneHandSwordSwing", true);
	}

	private void OneHandSwordReady()
	{
		OneHandSwordIdle();
		animator.SetBool("OneHandSwordReady", true);
		animator2.SetBool("OneHandSwordReady", true);
		animator3.SetBool("OneHandSwordReady", true);
	}

	private void OneHandSwordIdle()
	{
		Falses();
		oneHandSwordIdle = true;
		animator.SetBool("OneHandSwordIdle", true);
		animator2.SetBool("OneHandSwordIdle", true);
		animator3.SetBool("OneHandSwordIdle", true);
	}

	private void WeaponRun()
	{
		WeaponReady();
		weaponRun = true;
		animator.SetBool("WeaponRun", true);
		animator2.SetBool("WeaponRun", true);
		animator3.SetBool("WeaponRun", true);
	}

	private void PistolReload()
	{
		PistolReady();
		animator.SetBool("PistolReload", true);
		animator2.SetBool("PistolReload", true);
		animator3.SetBool("PistolReload", true);
	}

	private void PistolFire()
	{
		PistolInstant();
		animator.SetBool("PistolFire", true);
		animator2.SetBool("PistolFire", true);
		animator3.SetBool("PistolFire", true);
	}

	private void PistolInstant()
	{
		animator.SetBool("PistolInstant", true);
		animator2.SetBool("PistolInstant", true);
		animator3.SetBool("PistolInstant", true);
	}

	private void PistolReady()
	{
		Falses();
		pistolReadybool = true;
		animator.SetBool("PistolReady", true);
		animator2.SetBool("PistolReady", true);
		animator3.SetBool("PistolReady", true);
	}

	private void WeaponReload()
	{
		WeaponReady();
		animator.SetBool("WeaponReload", true);
		animator2.SetBool("WeaponReload", true);
		animator3.SetBool("WeaponReload", true);
	}

	private void WeaponFire()
	{
		WeaponInstant();
		animator.SetBool("WeaponFire", true);
		animator2.SetBool("WeaponFire", true);
		animator3.SetBool("WeaponFire", true);
	}

	private void ShotgunFire()
	{
		WeaponInstant();
		animator.SetBool("ShotgunFire", true);
		animator2.SetBool("ShotgunFire", true);
		animator3.SetBool("ShotgunFire", true);
	}

	private void ShotgunReloadChamber()
	{
		WeaponInstant();
		animator.SetBool("ShotgunReloadChamber", true);
		animator2.SetBool("ShotgunReloadChamber", true);
		animator3.SetBool("ShotgunReloadChamber", true);
		animator.SetBool("ShotgunReloadMagazine", true);
		animator2.SetBool("ShotgunReloadMagazine", true);
		animator3.SetBool("ShotgunReloadMagazine", true);
	}

	private void ShotgunReloadMagazine()
	{
		WeaponInstant();
		animator.SetBool("ShotgunReloadMagazine", true);
		animator2.SetBool("ShotgunReloadMagazine", true);
		animator3.SetBool("ShotgunReloadMagazine", true);
	}

	private void WeaponInstant()
	{
		WeaponReady();
		animator.SetBool("WeaponInstant", true);
		animator2.SetBool("WeaponInstant", true);
		animator3.SetBool("WeaponInstant", true);
	}

	private void NadeThrow()
	{
		WeaponInstant();
		animator.SetBool("NadeThrow", true);
		animator2.SetBool("NadeThrow", true);
		animator3.SetBool("NadeThrow", true);
	}

	private void WeaponReady()
	{
		Falses();
		weaponStandbool = true;
		animator.SetBool("WeaponReady", true);
		animator2.SetBool("WeaponReady", true);
		animator3.SetBool("WeaponReady", true);
	}

	private void WeaponStand()
	{
		Falses();
		weaponStandbool = true;
		animator.SetBool("WeaponStand", true);
		animator2.SetBool("WeaponStand", true);
		animator3.SetBool("WeaponStand", true);
	}

	private void RHandPunch()
	{
		animator.SetBool("RHandPunch", true);
		animator2.SetBool("RHandPunch", true);
		animator3.SetBool("RHandPunch", true);
	}

	private void LHandPunch()
	{
		animator.SetBool("LHandPunch", true);
		animator2.SetBool("LHandPunch", true);
		animator3.SetBool("LHandPunch", true);
	}

	private void FaceHit()
	{
		animator.SetBool("FaceHit", true);
		animator2.SetBool("FaceHit", true);
		animator3.SetBool("FaceHit", true);
	}

	private void FrontKick()
	{
		animator.SetBool("FrontKick", true);
		animator2.SetBool("FrontKick", true);
		animator3.SetBool("FrontKick", true);
	}

	private void IdleStand()
	{
		Falses();
		idleStandbool = true;
		animator.SetBool("IdleStand", true);
		animator2.SetBool("IdleStand", true);
		animator3.SetBool("IdleStand", true);
	}

	private void IdleReady()
	{
		Falses();
		idleReadybool = true;
		animator.SetBool("IdleReady", true);
		animator2.SetBool("IdleReady", true);
		animator3.SetBool("IdleReady", true);
	}

	private void IdleMonster()
	{
		Falses();
		idleMonsterbool = true;
		animator.SetBool("IdleMonster", true);
		animator2.SetBool("IdleMonster", true);
		animator3.SetBool("IdleMonster", true);
	}

	private void IdleCheer()
	{
		IdleStand();
		animator.SetBool("IdleCheer", true);
		animator2.SetBool("IdleCheer", true);
		animator3.SetBool("IdleCheer", true);
	}

	private void IdleWalk()
	{
		IdleStand();
		animator.SetBool("IdleWalk", true);
		animator2.SetBool("IdleWalk", true);
		animator3.SetBool("IdleWalk", true);
	}

	private void CratePush()
	{
		IdleStand();
		animator.SetBool("CratePush", true);
		animator2.SetBool("CratePush", true);
		animator3.SetBool("CratePush", true);
	}

	private void CratePull()
	{
		IdleStand();
		animator.SetBool("CratePull", true);
		animator2.SetBool("CratePull", true);
		animator3.SetBool("CratePull", true);
	}

	private void IdleStrafeRight()
	{
		IdleStand();
		animator.SetBool("IdleStrafeRight", true);
		animator2.SetBool("IdleStrafeRight", true);
		animator3.SetBool("IdleStrafeRight", true);
	}

	private void IdleStrafeLeft()
	{
		IdleStand();
		animator.SetBool("IdleStrafeLeft", true);
		animator2.SetBool("IdleStrafeLeft", true);
		animator3.SetBool("IdleStrafeLeft", true);
	}

	private void IdleRun()
	{
		IdleReady();
		animator.SetBool("IdleRun", true);
		animator2.SetBool("IdleRun", true);
		animator3.SetBool("IdleRun", true);
	}

	private void ComeHere()
	{
		IdleStand();
		animator.SetBool("ComeHere", true);
		animator2.SetBool("ComeHere", true);
		animator3.SetBool("ComeHere", true);
	}

	private void IdleKeepBack()
	{
		IdleStand();
		animator.SetBool("IdleKeepBack", true);
		animator2.SetBool("IdleKeepBack", true);
		animator3.SetBool("IdleKeepBack", true);
	}

	private void IdleFight()
	{
		Falses();
		idleFightbool = true;
		animator.SetBool("IdleFight", true);
		animator2.SetBool("IdleFight", true);
		animator3.SetBool("IdleFight", true);
	}

	private void IdleReadyCrouch()
	{
		IdleReady();
		animator.SetBool("IdleReadyCrouch", true);
		animator2.SetBool("IdleReadyCrouch", true);
		animator3.SetBool("IdleReadyCrouch", true);
	}

	private void IdleReadyLook()
	{
		IdleReady();
		animator.SetBool("IdleReadyLook", true);
		animator2.SetBool("IdleReadyLook", true);
		animator3.SetBool("IdleReadyLook", true);
	}

	private void IdleSad()
	{
		IdleStand();
		animator.SetBool("IdleSad", true);
		animator2.SetBool("IdleSad", true);
		animator3.SetBool("IdleSad", true);
	}

	private void IdleTurns()
	{
		IdleStand();
		animator.SetBool("IdleTurns", true);
		animator2.SetBool("IdleTurns", true);
		animator3.SetBool("IdleTurns", true);
	}

	private void Wizard1HandThrow()
	{
		IdleReady();
		animator.SetBool("Wizard1HandThrow", true);
		animator2.SetBool("Wizard1HandThrow", true);
		animator3.SetBool("Wizard1HandThrow", true);
	}

	private void Wizard2HandThrow()
	{
		IdleReady();
		animator.SetBool("Wizard2HandThrow", true);
		animator2.SetBool("Wizard2HandThrow", true);
		animator3.SetBool("Wizard2HandThrow", true);
	}

	private void WizardBlock()
	{
		IdleReady();
		animator.SetBool("WizardBlock", true);
		animator2.SetBool("WizardBlock", true);
		animator3.SetBool("WizardBlock", true);
	}

	private void WizardOverhead()
	{
		IdleReady();
		animator.SetBool("WizardOverhead", true);
		animator2.SetBool("WizardOverhead", true);
		animator3.SetBool("WizardOverhead", true);
	}

	private void WizardPowerUp()
	{
		IdleReady();
		animator.SetBool("WizardPowerUp", true);
		animator2.SetBool("WizardPowerUp", true);
		animator3.SetBool("WizardPowerUp", true);
	}

	private void IdleDie()
	{
		IdleReady();
		animator.SetBool("IdleDie", true);
		animator2.SetBool("IdleDie", true);
		animator3.SetBool("IdleDie", true);
	}

	private void Falses()
	{
		weaponStandbool = false;
		idleStandbool = false;
		idleReadybool = false;
		idleMonsterbool = false;
		idleFightbool = false;
		weaponRun = false;
		oneHandSwordIdle = false;
		pistolReadybool = false;
		bowIdle = false;
		motorbikeIdle = false;
		rollerBlade = false;
		skateboard = false;
		climbing = false;
		office = false;
		swim = false;
		wand = false;
		cards = false;
		breakdance = false;
		katana = false;
		soccer = false;
		giant = false;
		zombie = false;
		iceHockey = false;
		animator.SetBool("IceHockeyGoalieReady", false);
		animator2.SetBool("IceHockeyGoalieReady", false);
		animator3.SetBool("IceHockeyGoalieReady", false);
		animator.SetBool("IceHockeyDekeMiddle", false);
		animator2.SetBool("IceHockeyDekeMiddle", false);
		animator3.SetBool("IceHockeyDekeMiddle", false);
		animator.SetBool("IceHockeyIdle", false);
		animator2.SetBool("IceHockeyIdle", false);
		animator3.SetBool("IceHockeyIdle", false);
		animator.SetBool("1HandSwordStrafeLeft", false);
		animator2.SetBool("1HandSwordStrafeLeft", false);
		animator3.SetBool("1HandSwordStrafeLeft", false);
		animator.SetBool("1HandSwordStrafeRight", false);
		animator2.SetBool("1HandSwordStrafeRight", false);
		animator3.SetBool("1HandSwordStrafeRight", false);
		animator.SetBool("ZombieCrawl", false);
		animator2.SetBool("ZombieCrawl", false);
		animator3.SetBool("ZombieCrawl", false);
		animator.SetBool("ZombieWalk", false);
		animator2.SetBool("ZombieWalk", false);
		animator3.SetBool("ZombieWalk", false);
		animator.SetBool("ZombieIdle", false);
		animator2.SetBool("ZombieIdle", false);
		animator3.SetBool("ZombieIdle", false);
		animator.SetBool("WoodSaw", false);
		animator2.SetBool("WoodSaw", false);
		animator3.SetBool("WoodSaw", false);
		animator.SetBool("BlackSmithHammer", false);
		animator2.SetBool("BlackSmithHammer", false);
		animator3.SetBool("BlackSmithHammer", false);
		animator.SetBool("GiantGrabIdle2", false);
		animator2.SetBool("GiantGrabIdle2", false);
		animator3.SetBool("GiantGrabIdle2", false);
		animator.SetBool("GiantGrabIdle", false);
		animator2.SetBool("GiantGrabIdle", false);
		animator3.SetBool("GiantGrabIdle", false);
		animator.SetBool("WallSit", false);
		animator2.SetBool("WallSit", false);
		animator3.SetBool("WallSit", false);
		animator.SetBool("WallRunLeft", false);
		animator2.SetBool("WallRunLeft", false);
		animator3.SetBool("WallRunLeft", false);
		animator.SetBool("WallRunRight", false);
		animator2.SetBool("WallRunRight", false);
		animator3.SetBool("WallRunRight", false);
		animator.SetBool("ScubaSwim", false);
		animator2.SetBool("ScubaSwim", false);
		animator3.SetBool("ScubaSwim", false);
		animator.SetBool("BackPackOff", false);
		animator2.SetBool("BackPackOff", false);
		animator3.SetBool("BackPackOff", false);
		animator.SetBool("SneakForward", false);
		animator2.SetBool("SneakForward", false);
		animator3.SetBool("SneakForward", false);
		animator.SetBool("SneakBackward", false);
		animator2.SetBool("SneakBackward", false);
		animator3.SetBool("SneakBackward", false);
		animator.SetBool("SneakLeft", false);
		animator2.SetBool("SneakLeft", false);
		animator3.SetBool("SneakLeft", false);
		animator.SetBool("SneakRight", false);
		animator2.SetBool("SneakRight", false);
		animator3.SetBool("SneakRight", false);
		animator.SetBool("SneakIdle", false);
		animator2.SetBool("SneakIdle", false);
		animator3.SetBool("SneakIdle", false);
		animator.SetBool("SoccerRun", false);
		animator2.SetBool("SoccerRun", false);
		animator3.SetBool("SoccerRun", false);
		animator.SetBool("SoccerSprint", false);
		animator2.SetBool("SoccerSprint", false);
		animator3.SetBool("SoccerSprint", false);
		animator.SetBool("SoccerWalk", false);
		animator2.SetBool("SoccerWalk", false);
		animator3.SetBool("SoccerWalk", false);
		animator.SetBool("SoccerKeeperReady", false);
		animator2.SetBool("SoccerKeeperReady", false);
		animator3.SetBool("SoccerKeeperReady", false);
		animator.SetBool("Katana", false);
		animator2.SetBool("Katana", false);
		animator3.SetBool("Katana", false);
		animator.SetBool("KatanaReadyHigh", false);
		animator2.SetBool("KatanaReadyHigh", false);
		animator3.SetBool("KatanaReadyHigh", false);
		animator.SetBool("KatanaReady", false);
		animator2.SetBool("KatanaReady", false);
		animator3.SetBool("KatanaReady", false);
		animator.SetBool("KatanaReadyLow", false);
		animator2.SetBool("KatanaReadyLow", false);
		animator3.SetBool("KatanaReadyLow", false);
		animator.SetBool("KatanaReady", false);
		animator2.SetBool("KatanaReady", false);
		animator3.SetBool("KatanaReady", false);
		animator.SetBool("KneesIdle", false);
		animator2.SetBool("KneesIdle", false);
		animator3.SetBool("KneesIdle", false);
		animator.SetBool("WalkInjured", false);
		animator2.SetBool("WalkInjured", false);
		animator3.SetBool("WalkInjured", false);
		animator.SetBool("SatNightFever", false);
		animator2.SetBool("SatNightFever", false);
		animator3.SetBool("SatNightFever", false);
		animator.SetBool("RunningDance", false);
		animator2.SetBool("RunningDance", false);
		animator3.SetBool("RunningDance", false);
		animator.SetBool("RussianDance", false);
		animator2.SetBool("RussianDance", false);
		animator3.SetBool("RussianDance", false);
		animator.SetBool("ElvisLegsLoop", false);
		animator2.SetBool("ElvisLegsLoop", false);
		animator3.SetBool("ElvisLegsLoop", false);
		animator.SetBool("Flashlight", false);
		animator2.SetBool("Flashlight", false);
		animator3.SetBool("Flashlight", false);
		animator.SetBool("WalkBackward", false);
		animator2.SetBool("WalkBackward", false);
		animator3.SetBool("WalkBackward", false);
		animator.SetBool("Windmill", false);
		animator2.SetBool("Windmill", false);
		animator3.SetBool("Windmill", false);
		animator.SetBool("Flares", false);
		animator2.SetBool("Flares", false);
		animator3.SetBool("Flares", false);
		animator.SetBool("DeadmanFloat", false);
		animator2.SetBool("DeadmanFloat", false);
		animator3.SetBool("DeadmanFloat", false);
		animator.SetBool("2000", false);
		animator2.SetBool("2000", false);
		animator3.SetBool("2000", false);
		animator.SetBool("SixStep", false);
		animator2.SetBool("SixStep", false);
		animator3.SetBool("SixStep", false);
		animator.SetBool("ChannelCastOmni", false);
		animator2.SetBool("ChannelCastOmni", false);
		animator3.SetBool("ChannelCastOmni", false);
		animator.SetBool("ChannelCastDirected", false);
		animator2.SetBool("ChannelCastDirected", false);
		animator3.SetBool("ChannelCastDirected", false);
		animator.SetBool("BowInstant2", false);
		animator2.SetBool("BowInstant2", false);
		animator3.SetBool("BowInstant2", false);
		animator.SetBool("BowReady2", false);
		animator2.SetBool("BowReady2", false);
		animator3.SetBool("BowReady2", false);
		animator.SetBool("WalkDehydrated", false);
		animator2.SetBool("WalkDehydrated", false);
		animator3.SetBool("WalkDehydrated", false);
		animator.SetBool("UpHillWalk", false);
		animator2.SetBool("UpHillWalk", false);
		animator3.SetBool("UpHillWalk", false);
		animator.SetBool("CardPlayerIdle", false);
		animator2.SetBool("CardPlayerIdle", false);
		animator3.SetBool("CardPlayerIdle", false);
		animator.SetBool("DealerIdle", false);
		animator2.SetBool("DealerIdle", false);
		animator3.SetBool("DealerIdle", false);
		animator.SetBool("StaffStand", false);
		animator2.SetBool("StaffStand", false);
		animator3.SetBool("StaffStand", false);
		animator.SetBool("WandStand", false);
		animator2.SetBool("WandStand", false);
		animator3.SetBool("WandStand", false);
		animator.SetBool("SwimDogPaddle", false);
		animator2.SetBool("SwimDogPaddle", false);
		animator3.SetBool("SwimDogPaddle", false);
		animator.SetBool("SwimFreestyle", false);
		animator2.SetBool("SwimFreestyle", false);
		animator3.SetBool("SwimFreestyle", false);
		animator.SetBool("Swim", false);
		animator2.SetBool("Swim", false);
		animator3.SetBool("Swim", false);
		animator.SetBool("WateringCan", false);
		animator2.SetBool("WateringCan", false);
		animator3.SetBool("WateringCan", false);
		animator.SetBool("OfficeSittingReadingLeanBack", false);
		animator2.SetBool("OfficeSittingReadingLeanBack", false);
		animator3.SetBool("OfficeSittingReadingLeanBack", false);
		animator.SetBool("OfficeSittingReading", false);
		animator2.SetBool("OfficeSittingReading", false);
		animator3.SetBool("OfficeSittingReading", false);
		animator.SetBool("OfficeSittingLegCross", false);
		animator2.SetBool("OfficeSittingLegCross", false);
		animator3.SetBool("OfficeSittingLegCross", false);
		animator.SetBool("OfficeSittingBack", false);
		animator2.SetBool("OfficeSittingBack", false);
		animator3.SetBool("OfficeSittingBack", false);
		animator.SetBool("OfficeSitting45DegLeg", false);
		animator2.SetBool("OfficeSitting45DegLeg", false);
		animator3.SetBool("OfficeSitting45DegLeg", false);
		animator.SetBool("OfficeSitting1LegStraight", false);
		animator2.SetBool("OfficeSitting1LegStraight", false);
		animator3.SetBool("OfficeSitting1LegStraight", false);
		animator.SetBool("OfficeSitting", false);
		animator2.SetBool("OfficeSitting", false);
		animator3.SetBool("OfficeSitting", false);
		animator.SetBool("ClimbUp", false);
		animator2.SetBool("ClimbUp", false);
		animator3.SetBool("ClimbUp", false);
		animator.SetBool("ClimbIdle", false);
		animator2.SetBool("ClimbIdle", false);
		animator3.SetBool("ClimbIdle", false);
		animator.SetBool("SkateboardIdle", false);
		animator2.SetBool("SkateboardIdle", false);
		animator3.SetBool("SkateboardIdle", false);
		animator.SetBool("IdleFeedThrow", false);
		animator2.SetBool("IdleFeedThrow", false);
		animator3.SetBool("IdleFeedThrow", false);
		animator.SetBool("RollerBladeCrossoverLeft", false);
		animator2.SetBool("RollerBladeCrossoverLeft", false);
		animator3.SetBool("RollerBladeCrossoverLeft", false);
		animator.SetBool("RollerBladeTurnLeft", false);
		animator2.SetBool("RollerBladeTurnLeft", false);
		animator3.SetBool("RollerBladeTurnLeft", false);
		animator.SetBool("RollerBladeTurnRight", false);
		animator2.SetBool("RollerBladeTurnRight", false);
		animator3.SetBool("RollerBladeTurnRight", false);
		animator.SetBool("RollerBladeSkateFwd", false);
		animator2.SetBool("RollerBladeSkateFwd", false);
		animator3.SetBool("RollerBladeSkateFwd", false);
		animator.SetBool("RollerBladeCrossoverRight", false);
		animator2.SetBool("RollerBladeCrossoverRight", false);
		animator3.SetBool("RollerBladeCrossoverRight", false);
		animator.SetBool("RollerBladeGrindRoyale", false);
		animator2.SetBool("RollerBladeGrindRoyale", false);
		animator3.SetBool("RollerBladeGrindRoyale", false);
		animator.SetBool("RollerBladeRoll", false);
		animator2.SetBool("RollerBladeRoll", false);
		animator3.SetBool("RollerBladeRoll", false);
		animator.SetBool("RollerBladeStand", false);
		animator2.SetBool("RollerBladeStand", false);
		animator3.SetBool("RollerBladeStand", false);
		animator.SetBool("CrouchStrafeLeft", false);
		animator2.SetBool("CrouchStrafeLeft", false);
		animator3.SetBool("CrouchStrafeLeft", false);
		animator.SetBool("CrouchStrafeRight", false);
		animator2.SetBool("CrouchStrafeRight", false);
		animator3.SetBool("CrouchStrafeRight", false);
		animator.SetBool("CrouchWalkBackward", false);
		animator2.SetBool("CrouchWalkBackward", false);
		animator3.SetBool("CrouchWalkBackward", false);
		animator.SetBool("ProneLocomotion", false);
		animator2.SetBool("ProneLocomotion", false);
		animator3.SetBool("ProneLocomotion", false);
		animator.SetBool("ProneIdle", false);
		animator2.SetBool("ProneIdle", false);
		animator3.SetBool("ProneIdle", false);
		animator.SetBool("CrawlLocomotion", false);
		animator2.SetBool("CrawlLocomotion", false);
		animator3.SetBool("CrawlLocomotion", false);
		animator.SetBool("CrawlIdle", false);
		animator2.SetBool("CrawlIdle", false);
		animator3.SetBool("CrawlIdle", false);
		animator.SetBool("RunBackRight", false);
		animator2.SetBool("RunBackRight", false);
		animator3.SetBool("RunBackRight", false);
		animator.SetBool("RunBackLeft", false);
		animator2.SetBool("RunBackLeft", false);
		animator3.SetBool("RunBackLeft", false);
		animator.SetBool("WorkerShovel2", false);
		animator2.SetBool("WorkerShovel2", false);
		animator3.SetBool("WorkerShovel2", false);
		animator.SetBool("WorkerShovel", false);
		animator2.SetBool("WorkerShovel", false);
		animator3.SetBool("WorkerShovel", false);
		animator.SetBool("WorkerPickaxe", false);
		animator2.SetBool("WorkerPickaxe", false);
		animator3.SetBool("WorkerPickaxe", false);
		animator.SetBool("WorkerPickaxe2", false);
		animator2.SetBool("WorkerPickaxe2", false);
		animator3.SetBool("WorkerPickaxe2", false);
		animator.SetBool("WorkerHammer2", false);
		animator2.SetBool("WorkerHammer2", false);
		animator3.SetBool("WorkerHammer2", false);
		animator.SetBool("WorkerHammer", false);
		animator2.SetBool("WorkerHammer", false);
		animator3.SetBool("WorkerHammer", false);
		animator.SetBool("WoodCut", false);
		animator2.SetBool("WoodCut", false);
		animator3.SetBool("WoodCut", false);
		animator.SetBool("MotorbikeLasso", false);
		animator2.SetBool("MotorbikeLasso", false);
		animator3.SetBool("MotorbikeLasso", false);
		animator.SetBool("MotorbikeWheelyNoHands", false);
		animator2.SetBool("MotorbikeWheelyNoHands", false);
		animator3.SetBool("MotorbikeWheelyNoHands", false);
		animator.SetBool("MotorbikeWheely", false);
		animator2.SetBool("MotorbikeWheely", false);
		animator3.SetBool("MotorbikeWheely", false);
		animator.SetBool("MotorbikeSeatStandWheely", false);
		animator2.SetBool("MotorbikeSeatStandWheely", false);
		animator3.SetBool("MotorbikeSeatStandWheely", false);
		animator.SetBool("MotorbikeSeatStand", false);
		animator2.SetBool("MotorbikeSeatStand", false);
		animator3.SetBool("MotorbikeSeatStand", false);
		animator.SetBool("MotorbikeLookBack", false);
		animator2.SetBool("MotorbikeLookBack", false);
		animator3.SetBool("MotorbikeLookBack", false);
		animator.SetBool("MotorbikeHeartAttack", false);
		animator2.SetBool("MotorbikeHeartAttack", false);
		animator3.SetBool("MotorbikeHeartAttack", false);
		animator.SetBool("MotorbikeHeadstand", false);
		animator2.SetBool("MotorbikeHeadstand", false);
		animator3.SetBool("MotorbikeHeadstand", false);
		animator.SetBool("MotorbikeHandstand", false);
		animator2.SetBool("MotorbikeHandstand", false);
		animator3.SetBool("MotorbikeHandstand", false);
		animator.SetBool("MotorbikeHandlebarSit", false);
		animator2.SetBool("MotorbikeHandlebarSit", false);
		animator3.SetBool("MotorbikeHandlebarSit", false);
		animator.SetBool("MotorbikeBackwardStand", false);
		animator2.SetBool("MotorbikeBackwardStand", false);
		animator3.SetBool("MotorbikeBackwardStand", false);
		animator.SetBool("MotorbikeBackwardSitting", false);
		animator2.SetBool("MotorbikeBackwardSitting", false);
		animator3.SetBool("MotorbikeBackwardSitting", false);
		animator.SetBool("MotorbikeAirWalk", false);
		animator2.SetBool("MotorbikeAirWalk", false);
		animator3.SetBool("MotorbikeAirWalk", false);
		animator.SetBool("MotorbikeIdle", false);
		animator2.SetBool("MotorbikeIdle", false);
		animator3.SetBool("MotorbikeIdle", false);
		animator.SetBool("IdleStun", false);
		animator2.SetBool("IdleStun", false);
		animator3.SetBool("IdleStun", false);
		animator.SetBool("1HandSwordChargeUp", false);
		animator2.SetBool("1HandSwordChargeUp", false);
		animator3.SetBool("1HandSwordChargeUp", false);
		animator.SetBool("WeaponRunBackward", false);
		animator2.SetBool("WeaponRunBackward", false);
		animator3.SetBool("WeaponRunBackward", false);
		animator.SetBool("RunBackward", false);
		animator2.SetBool("RunBackward", false);
		animator3.SetBool("RunBackward", false);
		animator.SetBool("1HSwordStrafeRunRight", false);
		animator2.SetBool("1HSwordStrafeRunRight", false);
		animator3.SetBool("1HSwordStrafeRunRight", false);
		animator.SetBool("1HSwordStrafeRunLeft", false);
		animator2.SetBool("1HSwordStrafeRunLeft", false);
		animator3.SetBool("1HSwordStrafeRunLeft", false);
		animator.SetBool("WeaponStrafeRunRight", false);
		animator2.SetBool("WeaponStrafeRunRight", false);
		animator3.SetBool("WeaponStrafeRunRight", false);
		animator.SetBool("WeaponStrafeRunLeft", false);
		animator2.SetBool("WeaponStrafeRunLeft", false);
		animator3.SetBool("WeaponStrafeRunLeft", false);
		animator.SetBool("StrafeRunRight", false);
		animator2.SetBool("StrafeRunRight", false);
		animator3.SetBool("StrafeRunRight", false);
		animator.SetBool("StrafeRunLeft", false);
		animator2.SetBool("StrafeRunLeft", false);
		animator3.SetBool("StrafeRunLeft", false);
		animator.SetBool("FlyUp", false);
		animator2.SetBool("FlyUp", false);
		animator3.SetBool("FlyUp", false);
		animator.SetBool("FlyDown", false);
		animator2.SetBool("FlyDown", false);
		animator3.SetBool("FlyDown", false);
		animator.SetBool("FlyRight", false);
		animator2.SetBool("FlyRight", false);
		animator3.SetBool("FlyRight", false);
		animator.SetBool("FlyLeft", false);
		animator2.SetBool("FlyLeft", false);
		animator3.SetBool("FlyLeft", false);
		animator.SetBool("FlyBackward", false);
		animator2.SetBool("FlyBackward", false);
		animator3.SetBool("FlyBackward", false);
		animator.SetBool("FlyForward", false);
		animator2.SetBool("FlyForward", false);
		animator3.SetBool("FlyForward", false);
		animator.SetBool("IdleFly", false);
		animator2.SetBool("IdleFly", false);
		animator3.SetBool("IdleFly", false);
		animator.SetBool("IdleMeditate", false);
		animator2.SetBool("IdleMeditate", false);
		animator3.SetBool("IdleMeditate", false);
		animator.SetBool("ShotgunReloadMagazine", false);
		animator2.SetBool("ShotgunReloadMagazine", false);
		animator3.SetBool("ShotgunReloadMagazine", false);
		animator.SetBool("BowReady", false);
		animator2.SetBool("BowReady", false);
		animator3.SetBool("BowReady", false);
		animator.SetBool("BowInstant", false);
		animator2.SetBool("BowInstant", false);
		animator3.SetBool("BowInstant", false);
		animator.SetBool("BowFire", false);
		animator2.SetBool("BowFire", false);
		animator3.SetBool("BowFire", false);
		animator.SetBool("IdleStrafeLeft", false);
		animator2.SetBool("IdleStrafeLeft", false);
		animator3.SetBool("IdleStrafeLeft", false);
		animator.SetBool("IdleStrafeRight", false);
		animator2.SetBool("IdleStrafeRight", false);
		animator3.SetBool("IdleStrafeRight", false);
		animator.SetBool("CratePull", false);
		animator2.SetBool("CratePull", false);
		animator3.SetBool("CratePull", false);
		animator.SetBool("CratePush", false);
		animator2.SetBool("CratePush", false);
		animator3.SetBool("CratePush", false);
		animator.SetBool("IdleWalk", false);
		animator2.SetBool("IdleWalk", false);
		animator3.SetBool("IdleWalk", false);
		animator.SetBool("WeaponRun", false);
		animator2.SetBool("WeaponRun", false);
		animator3.SetBool("WeaponRun", false);
		animator.SetBool("WeaponStand", false);
		animator2.SetBool("WeaponStand", false);
		animator3.SetBool("WeaponStand", false);
		animator.SetBool("IdleReady", false);
		animator2.SetBool("IdleReady", false);
		animator3.SetBool("IdleReady", false);
		animator.SetBool("IdleStand", false);
		animator2.SetBool("IdleStand", false);
		animator3.SetBool("IdleStand", false);
		animator.SetBool("IdleMonster", false);
		animator2.SetBool("IdleMonster", false);
		animator3.SetBool("IdleMonster", false);
		animator.SetBool("WeaponReady", false);
		animator2.SetBool("WeaponReady", false);
		animator3.SetBool("WeaponReady", false);
		animator.SetBool("WeaponInstant", false);
		animator2.SetBool("WeaponInstant", false);
		animator3.SetBool("WeaponInstant", false);
		animator.SetBool("IdleFight", false);
		animator2.SetBool("IdleFight", false);
		animator3.SetBool("IdleFight", false);
		animator.SetBool("PistolReady", false);
		animator2.SetBool("PistolReady", false);
		animator3.SetBool("PistolReady", false);
		animator.SetBool("PistolInstant", false);
		animator2.SetBool("PistolInstant", false);
		animator3.SetBool("PistolInstant", false);
		animator.SetBool("OneHandSwordIdle", false);
		animator2.SetBool("OneHandSwordIdle", false);
		animator3.SetBool("OneHandSwordIdle", false);
		animator.SetBool("OneHandSwordReady", false);
		animator2.SetBool("OneHandSwordReady", false);
		animator3.SetBool("OneHandSwordReady", false);
		animator.SetBool("OneHandSwordRun", false);
		animator2.SetBool("OneHandSwordRun", false);
		animator3.SetBool("OneHandSwordRun", false);
		animator.SetBool("IdleRun", false);
		animator2.SetBool("IdleRun", false);
		animator3.SetBool("IdleRun", false);
		animator.SetBool("BowIdle", false);
		animator2.SetBool("BowIdle", false);
		animator3.SetBool("BowIdle", false);
		animator.SetBool("IdleReadyCrouch", false);
		animator2.SetBool("IdleReadyCrouch", false);
		animator3.SetBool("IdleReadyCrouch", false);
		animator.SetBool("CrouchWalk", false);
		animator2.SetBool("CrouchWalk", false);
		animator3.SetBool("CrouchWalk", false);
	}

	private void Update()
	{
		if (animator.GetFloat("Curve") > 0.1f)
		{
			if (animator.GetBool("BowFire"))
			{
				BowReady();
			}
			animator.SetBool("Giant2HandSlamIdle", false);
			animator2.SetBool("Giant2HandSlamIdle", false);
			animator3.SetBool("Giant2HandSlamIdle", false);
			animator.SetBool("GiantGrabIdle2", false);
			animator2.SetBool("GiantGrabIdle2", false);
			animator3.SetBool("GiantGrabIdle2", false);
			animator.SetBool("GiantGrabIdle", false);
			animator2.SetBool("GiantGrabIdle", false);
			animator3.SetBool("GiantGrabIdle", false);
			animator.SetBool("WateringCanWatering", false);
			animator2.SetBool("WateringCanWatering", false);
			animator3.SetBool("WateringCanWatering", false);
			animator.SetBool("OfficeSittingReadingPageFlip", false);
			animator2.SetBool("OfficeSittingReadingPageFlip", false);
			animator3.SetBool("OfficeSittingReadingPageFlip", false);
			animator.SetBool("OfficeSittingEyesRub", false);
			animator2.SetBool("OfficeSittingEyesRub", false);
			animator3.SetBool("OfficeSittingEyesRub", false);
			animator.SetBool("OfficeSittingHandRestFingerTap", false);
			animator2.SetBool("OfficeSittingHandRestFingerTap", false);
			animator3.SetBool("OfficeSittingHandRestFingerTap", false);
			animator.SetBool("OfficeSittingMouseMovement", false);
			animator2.SetBool("OfficeSittingMouseMovement", false);
			animator3.SetBool("OfficeSittingMouseMovement", false);
			animator.SetBool("OfficeSittingReadingCoffeeSip", false);
			animator2.SetBool("OfficeSittingReadingCoffeeSip", false);
			animator3.SetBool("OfficeSittingReadingCoffeeSip", false);
			animator.SetBool("VaderChoke", false);
			animator2.SetBool("VaderChoke", false);
			animator3.SetBool("VaderChoke", false);
			animator.SetBool("HeelClick", false);
			animator2.SetBool("HeelClick", false);
			animator3.SetBool("HeelClick", false);
			animator.SetBool("Yawn", false);
			animator2.SetBool("Yawn", false);
			animator3.SetBool("Yawn", false);
			animator.SetBool("360SpinDeath", false);
			animator2.SetBool("360SpinDeath", false);
			animator3.SetBool("360SpinDeath", false);
			animator.SetBool("ClimbLeft", false);
			animator2.SetBool("ClimbLeft", false);
			animator3.SetBool("ClimbLeft", false);
			animator.SetBool("ClimbRight", false);
			animator2.SetBool("ClimbRight", false);
			animator3.SetBool("ClimbRight", false);
			animator.SetBool("SkateboardKickPush", false);
			animator2.SetBool("SkateboardKickPush", false);
			animator3.SetBool("SkateboardKickPush", false);
			animator.SetBool("IdleStandingJump", false);
			animator2.SetBool("IdleStandingJump", false);
			animator3.SetBool("IdleStandingJump", false);
			animator.SetBool("IdleSlide", false);
			animator2.SetBool("IdleSlide", false);
			animator3.SetBool("IdleSlide", false);
			animator.SetBool("RollerBladeFrontFlip", false);
			animator2.SetBool("RollerBladeFrontFlip", false);
			animator3.SetBool("RollerBladeFrontFlip", false);
			animator.SetBool("RollerBladeBackFlip", false);
			animator2.SetBool("RollerBladeBackFlip", false);
			animator3.SetBool("RollerBladeBackFlip", false);
			animator.SetBool("RollerBladeStop", false);
			animator2.SetBool("RollerBladeStop", false);
			animator3.SetBool("RollerBladeStop", false);
			animator.SetBool("RollerBladeJump", false);
			animator2.SetBool("RollerBladeJump", false);
			animator3.SetBool("RollerBladeJump", false);
			animator.SetBool("MotorbikeSuperman", false);
			animator2.SetBool("MotorbikeSuperman", false);
			animator3.SetBool("MotorbikeSuperman", false);
			animator.SetBool("MotorbikeSpecialFlip", false);
			animator2.SetBool("MotorbikeSpecialFlip", false);
			animator3.SetBool("MotorbikeSpecialFlip", false);
			animator.SetBool("PistolLeftHandStab", false);
			animator2.SetBool("PistolLeftHandStab", false);
			animator3.SetBool("PistolLeftHandStab", false);
			animator.SetBool("IdleMouthWipe", false);
			animator2.SetBool("IdleMouthWipe", false);
			animator3.SetBool("IdleMouthWipe", false);
			animator.SetBool("IdleSpew", false);
			animator2.SetBool("IdleSpew", false);
			animator3.SetBool("IdleSpew", false);
			animator.SetBool("1HandSwordRollAttack", false);
			animator2.SetBool("1HandSwordRollAttack", false);
			animator3.SetBool("1HandSwordRollAttack", false);
			animator.SetBool("MotorbikeTurnRight", false);
			animator2.SetBool("MotorbikeTurnRight", false);
			animator3.SetBool("MotorbikeTurnRight", false);
			animator.SetBool("MotorbikeTurnLeft", false);
			animator2.SetBool("MotorbikeTurnLeft", false);
			animator3.SetBool("MotorbikeTurnLeft", false);
			animator.SetBool("MotorbikeShootLeft", false);
			animator2.SetBool("MotorbikeShootLeft", false);
			animator3.SetBool("MotorbikeShootLeft", false);
			animator.SetBool("MotorbikeShootRight", false);
			animator2.SetBool("MotorbikeShootRight", false);
			animator3.SetBool("MotorbikeShootRight", false);
			animator.SetBool("MotorbikeShootFwd", false);
			animator2.SetBool("MotorbikeShootFwd", false);
			animator3.SetBool("MotorbikeShootFwd", false);
			animator.SetBool("MotorbikeShootBack", false);
			animator2.SetBool("MotorbikeShootBack", false);
			animator3.SetBool("MotorbikeShootBack", false);
			animator.SetBool("MotorbikeLassoRight", false);
			animator2.SetBool("MotorbikeLassoRight", false);
			animator3.SetBool("MotorbikeLassoRight", false);
			animator.SetBool("MotorbikeLassoLeft", false);
			animator2.SetBool("MotorbikeLassoLeft", false);
			animator3.SetBool("MotorbikeLassoLeft", false);
			animator.SetBool("MotorbikeLassoBack", false);
			animator2.SetBool("MotorbikeLassoBack", false);
			animator3.SetBool("MotorbikeLassoBack", false);
			animator.SetBool("MotorbikeLassoFwd", false);
			animator2.SetBool("MotorbikeLassoFwd", false);
			animator3.SetBool("MotorbikeLassoFwd", false);
			animator.SetBool("MotorbikeBackwardSittingCheer", false);
			animator2.SetBool("MotorbikeBackwardSittingCheer", false);
			animator3.SetBool("MotorbikeBackwardSittingCheer", false);
			animator.SetBool("WeaponReadyFire", false);
			animator2.SetBool("WeaponReadyFire", false);
			animator3.SetBool("WeaponReadyFire", false);
			animator.SetBool("ShotgunReadyFire", false);
			animator2.SetBool("ShotgunReadyFire", false);
			animator3.SetBool("ShotgunReadyFire", false);
			animator.SetBool("1HandSwordChargeUp", false);
			animator2.SetBool("1HandSwordChargeUp", false);
			animator3.SetBool("1HandSwordChargeUp", false);
			animator.SetBool("1HandSwordChargeSwipe", false);
			animator2.SetBool("1HandSwordChargeSwipe", false);
			animator3.SetBool("1HandSwordChargeSwipe", false);
			animator.SetBool("1HandSwordChargeHeavyBash", false);
			animator2.SetBool("1HandSwordChargeHeavyBash", false);
			animator3.SetBool("1HandSwordChargeHeavyBash", false);
			animator.SetBool("1HandSwordShieldBash", false);
			animator2.SetBool("1HandSwordShieldBash", false);
			animator3.SetBool("1HandSwordShieldBash", false);
			animator.SetBool("Crouch180", false);
			animator2.SetBool("Crouch180", false);
			animator3.SetBool("Crouch180", false);
			animator.SetBool("WizardNeoBlock", false);
			animator2.SetBool("WizardNeoBlock", false);
			animator3.SetBool("WizardNeoBlock", false);
			animator.SetBool("WizardEyeBeam", false);
			animator2.SetBool("WizardEyeBeam", false);
			animator3.SetBool("WizardEyeBeam", false);
			animator.SetBool("ShotgunFire", false);
			animator2.SetBool("ShotgunFire", false);
			animator3.SetBool("ShotgunFire", false);
			animator.SetBool("IdleDodgeLeft", false);
			animator2.SetBool("IdleDodgeLeft", false);
			animator3.SetBool("IdleDodgeLeft", false);
			animator.SetBool("IdleDodgeRight", false);
			animator2.SetBool("IdleDodgeRight", false);
			animator3.SetBool("IdleDodgeRight", false);
			animator.SetBool("RunDive", false);
			animator2.SetBool("RunDive", false);
			animator3.SetBool("RunDive", false);
			animator.SetBool("RunJump", false);
			animator2.SetBool("RunJump", false);
			animator3.SetBool("RunJump", false);
			animator.SetBool("Cowboy1HandDraw", false);
			animator2.SetBool("Cowboy1HandDraw", false);
			animator3.SetBool("Cowboy1HandDraw", false);
			animator.SetBool("WeaponReload", false);
			animator2.SetBool("WeaponReload", false);
			animator3.SetBool("WeaponReload", false);
			animator.SetBool("WeaponFire", false);
			animator2.SetBool("WeaponFire", false);
			animator3.SetBool("WeaponFire", false);
			animator.SetBool("RHandPunch", false);
			animator2.SetBool("RHandPunch", false);
			animator3.SetBool("RHandPunch", false);
			animator.SetBool("LHandPunch", false);
			animator2.SetBool("LHandPunch", false);
			animator3.SetBool("LHandPunch", false);
			animator.SetBool("FaceHit", false);
			animator2.SetBool("FaceHit", false);
			animator3.SetBool("FaceHit", false);
			animator.SetBool("FrontKick", false);
			animator2.SetBool("FrontKick", false);
			animator3.SetBool("FrontKick", false);
			animator.SetBool("IdleCheer", false);
			animator2.SetBool("IdleCheer", false);
			animator3.SetBool("IdleCheer", false);
			animator.SetBool("ComeHere", false);
			animator2.SetBool("ComeHere", false);
			animator3.SetBool("ComeHere", false);
			animator.SetBool("IdleKeepBack", false);
			animator2.SetBool("IdleKeepBack", false);
			animator3.SetBool("IdleKeepBack", false);
			animator.SetBool("IdleReadyLook", false);
			animator2.SetBool("IdleReadyLook", false);
			animator3.SetBool("IdleReadyLook", false);
			animator.SetBool("IdleSad", false);
			animator2.SetBool("IdleSad", false);
			animator3.SetBool("IdleSad", false);
			animator.SetBool("Wizard1HandThrow", false);
			animator2.SetBool("Wizard1HandThrow", false);
			animator3.SetBool("Wizard1HandThrow", false);
			animator.SetBool("Wizard2HandThrow", false);
			animator2.SetBool("Wizard2HandThrow", false);
			animator3.SetBool("Wizard2HandThrow", false);
			animator.SetBool("WizardBlock", false);
			animator2.SetBool("WizardBlock", false);
			animator3.SetBool("WizardBlock", false);
			animator.SetBool("WizardOverhead", false);
			animator2.SetBool("WizardOverhead", false);
			animator3.SetBool("WizardOverhead", false);
			animator.SetBool("WizardPowerUp", false);
			animator2.SetBool("WizardPowerUp", false);
			animator3.SetBool("WizardPowerUp", false);
			animator.SetBool("PistolFire", false);
			animator2.SetBool("PistolFire", false);
			animator3.SetBool("PistolFire", false);
			animator.SetBool("PistolReload", false);
			animator2.SetBool("PistolReload", false);
			animator3.SetBool("PistolReload", false);
			animator.SetBool("OneHandSwordSwing", false);
			animator2.SetBool("OneHandSwordSwing", false);
			animator3.SetBool("OneHandSwordSwing", false);
			animator.SetBool("OneHandSwordBackSwing", false);
			animator2.SetBool("OneHandSwordBackSwing", false);
			animator3.SetBool("OneHandSwordBackSwing", false);
			animator.SetBool("OneHandSwordJab", false);
			animator2.SetBool("OneHandSwordJab", false);
			animator3.SetBool("OneHandSwordJab", false);
			animator.SetBool("OneHandSwordBlock", false);
			animator2.SetBool("OneHandSwordBlock", false);
			animator3.SetBool("OneHandSwordBlock", false);
			animator.SetBool("IdleDie", false);
			animator2.SetBool("IdleDie", false);
			animator3.SetBool("IdleDie", false);
			animator.SetBool("IdleTurns", false);
			animator2.SetBool("IdleTurns", false);
			animator3.SetBool("IdleTurns", false);
			animator.SetBool("ShotgunReloadChamber", false);
			animator2.SetBool("ShotgunReloadChamber", false);
			animator3.SetBool("ShotgunReloadChamber", false);
			animator.SetBool("NadeThrow", false);
			animator2.SetBool("NadeThrow", false);
			animator3.SetBool("NadeThrow", false);
			animator.SetBool("Idle180", false);
			animator2.SetBool("Idle180", false);
			animator3.SetBool("Idle180", false);
			animator.SetBool("IdleButtonPress", false);
			animator2.SetBool("IdleButtonPress", false);
			animator3.SetBool("IdleButtonPress", false);
			animator.SetBool("IdleTyping", false);
			animator2.SetBool("IdleTyping", false);
			animator3.SetBool("IdleTyping", false);
		}
	}
}
