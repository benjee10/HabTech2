HABTECH 2 1.1.0 DEV VERSION
A mod for Kerbal Space Program [Breaking Ground Expansion] 1.12.x by Benjee10. 

Featuring parts designed to replicate the US Orbital Section (USOS) of the International Space Station, including:
- Pressurised modules & cupola
- Integrated Truss Structure & solar arrays, including iROSA
- External payloads & experiments
- Other structural pieces, science parts and antennas
- Canadarm2 & Mobile Base System
- New additions to the ISS such as BEAM, Bishop airlock, and more

Tantares is recommended to build the Russian Orbital Section (ROS) 

CRAFT FILES
The following craft files are included: 
- International Space Station - USOS (2023)
- International Space Station (2023)*
- International Space Station (2011)*
- International Space Station (2003)*
- Space Station Freedom (1991 baseline)

* = requires Tantares for ROS parts. ROS craft files by Zarbon

INSTALLATION
Place the contents of GameData/ into your KSP/GameData/ directory. 
If you already have HabTech2, benjee10_sharedAssets or htRobotics installed, please delete the existing mod folders in your GameData directory before installing the new versions to avoid compatibility issues.

DEPENDENCIES
All hard dependencies are bundled with the release:

- Module Manager
- Community Resource Pack
- B9PartSwitch
- Benjee10 Shared Assets
- HabTech Props
- HabTech Robotics

You will also need Kerbal Space Program Breaking Ground DLC for robotics parts. 

COMPATIBILITY
- Shaddy & Textures Unlimited - provides translucent shaders for solar array parts
- Neptune Camera - provides camera functionality for camera mast part
- Waterfall - Waterfall FX are provided for lights and RCS thrusters
- Limited support for TAC LS
- Community Category Kit - adapted from configs by Lennon


CHANGELOG

1.1.0 - DEV VERSION
	- Fixed issue with some truss connector ports lacking a dockingNode transform
	- Added InventoryUpgrades plugin by ValiZockt
	- iROSA panels now added via InvetoryUpgrade system; iROSA part can be dragged into inventory upgrade slot on SAW parts to add it to that part. Standalone SAW has a single slot, P/S3 & P/S6 have double slot
	- Updated craft files

1.0.0 - FINAL MAJOR UPDATE
	REVAMPED PARTS
	- Revamped Integrated Truss Structure Parts (S0, P/S1, P/S3)
	- Revamped Z1 Truss part
	- Revamped Dual-Axis Solar Arrays & added additional variants
	- Revamped ITS Radiator part
	- Revamped Pressurised Mating Adapter part

	NEW PARTS
	- Added iROSA panels
	- Added Japanese Experiment Module external experiment parts (x5)
	- Added Japanese Experiment Module science docking port
	- Added Bigelow Expandable Activities Module part
	- Added NanoRacks Bishop Airlock part
	- Added additional Integrated Truss Structure parts
	- Added additional truss connection system parts
	- Added Space Station Freedom Propulsion Module & Propellant Module parts
	- Added Express Logistics Carrier (ELC) and External Stowage Platform (ESP) parts
	- Added Alpha Magnetic Spectrometer part
	- Added Z1 swing and fixed SGANT antenna parts
	- Added External Camera & Lighting Assembly mast part
	- Added WETA Antenna mast part
	- Added FPMU Science mast part
	- Added S-Band Antenna mast part
	- Added UHF Boom Antenna part
	- Added Ammonia Tank Assembly part
	- Added ISS-style battery part
	- Added ISS-style strut part
	- Added Mobile Transporter & Mobile Base System parts
	- Added spherical node modules (1.875m)

	INTERNALS
	- Added simple IVAs for all pressurised modules

	IMPROVEMENTS
	- Added white variant to pressurised modules
	- Improved handrail model on all parts
	- Added blue handrail variant on all parts
	- Added mount variants for Japanese Experiment Module Exposed Facility
	- Added iROSA panels as part switches for SAW parts

	FIXES
	- Fixed broken normals on Cupola IVA
	- Adjusted part balance
	- Adjusted names, tags and descriptions for clarity
	- Corrected orientation of JEM Logistics module

	DEPENDENCIES
	- Removed old Common Berthing Mechanism part
	- Updated Benjee10_sharedAssets:
		- Added revamped Common Berthing Mechanism (CBM) parts - Passive, Active I and Active II
		- Added hybrid APAS/SSVP ports (male & female) used on ROS
	- Updated HabTechRobotics:
		- Added Canadarm2 Boom, Servo and Latching End Effector parts
		- Revamped Power-Data Grapple Fixture (PDGF) part and added additional variants
		- Added JEM RMS parts
		- Adjusted traverse rates and part balance
		- Texture improvements
	- Bundled Benjee10_sharedAssets
	- Bundled htRobotics

	COMPATIBILITY
	- Added support for Shaddy shaders via TU
	- Added support for Neptune Camera
	- Added support for Custom Category Kit [TBA]
	- Deprecated revamped parts where a drop-in replacement is not possible
	- CBM ports on legacy craft will become CBM (passive) type

	CRAFT FILES
	- Added craft files:
		- INTERNATIONAL SPACE STATION US ORBITAL SECTION (2023)
		- SPACE STATION FREEDOM (1991)

	- Added craft files featuring Tantares:
		- INTERNATIONAL SPACE STATION (2003)
		- INTERNATIONAL SPACE STATION (2011)
		- INTERNATIONAL SPACE STATION (2023)

0.2.5 - PUBLIC RELEASE
	- Updated Benjee10_sahredAssets (added new APAS model, deprecated old APAS part)
	- Fixed radiator drag cubes
	- Added stock inventory support. CBM & APAS not currently constructible due to stock bug. 

0.2.4 - PUBLIC RELEASE
	- Fixed solar array spawn bug!
	- Updated Benjee10_sharedAssets

0.2.3 - DEV RELEASE
	- Added half-length MPLM part
	- Corrected category for MPLM part, Science -> Payload
	- Amended tags for MPLM part
	- Added additional variants for MPLM part
	- Amended drag cubes on Solar Array Wing to allow game loading (TEMPORARY FIX)
	- Set physicsSignificance to 1 on Solar Array Wing to avoid spontaneous combustion (TEMPORARY FIX)
	- Converted all textures to DDS

0.2.2 - DEV RELEASE
	- Added JEM exposed facility. 
	- Fixed NRE on load.

0.2.1 - PRE-RELEASE
	- Added IVA for Cupola module. 
	- Bundled HabTechProps. 
	- Added patch for TAC LS. 

0.2.0 - PUBLIC RELEASE
	- Added bearing variant for truss connector port. 
	- Added Z1 truss.
	- Updated textures for S5/S6 truss elements.

0.1.9 - PRE-RELEASE
	- Added standalone solar array part. 
	- Added triple radiator part. 

0.1.8 - PRE-RELEASE
	- Added part variants to Truss Connector Port. 
	- Added Dual Solar Array Wing w/ integrated radiator. 

0.1.7 - PRE-RELEASE
	- Added S0 Truss.
	- Added P1/S1 Truss.
	- Added P2/S2 Truss.
	- Added Truss Connector Port.
	- Added Radial Truss Connector Port. 

0.1.6 - PUBLIC RELEASE - USOS PRESSURISED COMPONENT COMPLETE
	- Added Cupola module
	- Config polish
	- Collider polish on most parts
	- Tweaked science lab values
	- Tweaked thermal values
	- Tweaked costs
	- Added ladder colliders to most parts

0.1.5 - PRE-RELEASE
	- Added Laboratory Node.
	- Added Kibo Laboratory.
	- Added Kibo Logistics module.
	- Added MPLM.
	- Corrected rotation of some existing parts.
	- Rebalanced thermal tolerances. 

0.1.4 - PRE-RELEASE
	- Added Harmony module.
	- Added Columbus laboratory module. 

0.1.3 - INITIAL PUBLIC RELEASE
	- Tweaked part .CFGs
	- Added Quest pod mounting rack
	- Added switchable resources for Quest pod
	- Configured Quest pod for KIS
	- KIS is now a dependency

0.1.2 - PRE-RELEASE
	- Bundled APAS with Benjee10_sharedAssets

0.1.1 - PRE-RELEASE
	- Added Pressurised Mating Adapter
	- Adjusted normals on Quest RCS pod
	- Added switchable numerical decals on Quest RCS pod

0.1.0 - PRE-RELEASE
	Initial release. 
	- Added Unity module
	- Added Destiny module
	- Added Quest Airlock
	- Added radial RCS pod
	- Added standalone handrail (x1, x2 & x3)
	- Added Common Berthing Mechanism

LICENSE & CREDITS
HabTech2 artwork is copyright Benedict Jewer 2019-2023. All Rights Reserved. 
InvetoryUpgrades plugin by ValiZockt
Dependencies are bundled under their respective licenses. 