/*


@startuml
skinparam classAttributeIconSize 0
class GameController
{
    +StartLevel(int id)
    +ChangePhase(LevelPhase p)
}
enum LevelPhase
{
    Generate
    SelectClass
    SelectSubClass
    PlaceHeroes
    Combat
    EndLevel
}
class LevelLoader
{
    +LoadLevelFromResources(int id) : LevelData
    +GenerateLevel(LevelData data)
}
class LevelData
{
    +int ID
    +int playerSpawnRows
    +int selectableHeroes
    +float placingTime
    +int[] dimensions
    +List<string> grid
    +List<string> enemies
}

' =============================

class TileGrid
{
    +Tile[,] tiles
    +Vector2Int dimensions
    +Initialize(int w, int h)
    +GetTile(int x, int y) : Tile
    +WorldToTile(Vector3 pos) : Tile
}

class Tile
{
    +int x
    +int y
    +Vector3 groundPoint
    +bool walkable
    +Entity occupant
    +List<Tile> neighbors
    +bool IsFree()
}

TileGrid-- > Tile : contains >

' =============================

abstract class Entity
{
    +EntityData data
    +Tile currentTile
    +Initialize(EntityData data)
    +OnTick(int tick)
}

class Hero
class Enemy

Hero -|> Entity
Enemy -|> Entity

class EntityData << (S,#FFAA00) >> {
    +string id
    + GameObject prefab
    + int baseHp
    + int baseAtk
    + AbilityData[] abilities
}

Entity-- > EntityData

' Components (optional expansion)
class HealthComponent
{
    +int hp
    +void Damage(int amount)
}
class MovementComponent
{
    +void MoveTo(Tile target)
}
class CombatComponent
{
    +void PerformAttack(Entity target)
}
class AIComponent
{
    +void DecideAction()
}

Entity-- > HealthComponent
Entity-- > MovementComponent
Entity-- > CombatComponent
Entity-- > AIComponent

' =============================
'        ABILITY SYSTEM
' =============================

interface IAbility
{
    +CanCast(Entity caster, Tile target) : bool
    +Execute(Entity caster, Tile target)
}

class AbilityData << (S,#99FF99) >> {
    +string abilityId
    + int range
    + float cooldown
}

AbilityData-- > IAbility : implemented by >

' =============================
'       TICK SYSTEM
' =============================

class TickManager
{
    +float tickRate
    +int CurrentTick
    +event OnTick
    +Start()
    +Stop()
}

TickManager-- > Entity : calls OnTick()

' =============================
'       LEVEL FLOW SYSTEM
' =============================

class PlacementPhaseManager
{
    +float placingTime
    +BeginPlacement(float t)
    +TryPlace(Card card, Tile tile)
}

class CombatManager
{
    +ResolveActions()
    +QueueAction(Entity e, Action a)
}

PlacementPhaseManager-- > TileGrid
CombatManager-- > Entity

' =============================
'       CARD & HERO SELECTION
' =============================

class Deck
{
    +List<Card> cards
    +Draw(int n) : List<Card>
}

class Card
{
    +HeroData heroData
}

class HeroData << (S,#FFDD88)>> {
    +string id
    + EntityData stats
    + Sprite portrait
}

Deck-- > Card
Card-- > HeroData
HeroData-- > EntityData

' =============================
'          RELATIONS
' =============================

GameController-- > LevelLoader
GameController-- > PlacementPhaseManager
GameController-- > CombatManager
GameController-- > TickManager

LevelLoader-- > TileGrid
LevelLoader-- > Tile : instantiates
LevelLoader --> Enemy : spawns via prefab

PlacementPhaseManager --> Deck

Tile --> Entity : occupant
Entity --> Tile : position

@enduml
*/