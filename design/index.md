# Game 2
---

## Summary

Game 2 is a top-down action game where you build magic circuits in the world to open doors that lead deeper and deeper into the dungeon.

## Magic Circuits

Magic circuits are constructed by placing magiparts into the world.  Magiparts are analagous to real-world analog electronic parts, like resistors and capacitors. Magiparts are connected by Trace.  Trace is analogous to wires in real-world electronics. Connecting a part to trace that is carrying more mana, or an incompatable mana type will cause the part to take damage.  

### Magiparts List

#### Basic Circuits

| Part            | Cost  | HP  | Max Mana Input |  Description  |
| ----            | ----- | --- | ---               | ---           |
| Trace           | 1     | 10  | 5   | Connects other parts. Carries between 1 and 5 mana of any color.
| Heavy Trace     | 50    | 10  | 50  | Connects other parts. Carries between 5 and 50 mana of any color.
| Red Trace       | 1     | 20  | 5R  | Connects other parts. Carries only red mana. Transmitting other colors causes one section of trace to take damage |
| Blue Trace      | 1     | 20  | 5B  | Connects other parts. Carries only blue mana. Transmitting other colors causes one section of trace to take damage |
| Green Trace     | 1     | 20  | 5G  | Connects other parts. Carries only green mana. Transmitting other colors causes one section of trace to take damage |
| Sm Resistor     | 2     | 5   | 20  | Limits mana to 5 |
| L Resistor      | 5     | 20  | 50  | Limits mana to 20 |
| Sm Capacitor    | 2     | 5   | 5   | input mana until 5 is stored, then emits 5 until 5 is consumed
| L Capacitor     | 10    | 10  | 20  | Input mana until 20 is stored, then emits 20 until 20 is consumed
| Prism           | 20    | 10  | 15  | Split colorless mana into equal ammounts of R G B mana
| Sm Mana Battery | 20    | 10  | 5   | Stores mana on the input, emits 1 mana on the output.
| L Mana Battery  | 50    | 20  | 5   | Stores mana on the input, emits 5 mana on the output.
| Mana Generator  | 100   | 20  | 0   | Emits 1 mana on the output
| R Mana Generator| 500   | 20  | 0   | Emits 1R mana on the output
| G Mana Generator| 500   | 20  | 0   | Emits 1G mana on the output
| B Mana Generator| 500   | 20  | 0   | Emits 1B mana on the output

#### Environmental
| Part            | Cost  | HP  | Max Mana Input |  Description  |
| ----            | ----- | --- | ---               | ---           |
| Large Mana Generator | ----- | --- | ---               | ---           |
| Door | ----- | --- | ---               | ---           |
| Spike Trap | ----- | --- | ---               | ---           |

#### Tower Defense
| Part            | Cost  | HP  | Max Mana Input |  Description  |
| ----            | ----- | --- | ---               | ---           |
| MM Turret       | 50    | 20  | 5   | Fires homing projectiles
| Flame Turret    | 50    | 20  | 5R  | Emits an arc of flame, and turns ice floor into water
| Ice Turret      | 50    | 20  | 5B  | Slows enemies within its radius, and solidifies Lava
| Healing Turret  | 50    | 20  | 1G  | Heals player and all parts in its radius, and causes plants to grow
| Thumper         | 50    | 20  | 5   | Attracts monsters in its radius
| Barrier         | 50    | 200 | 5   | Heals itself over time
| Bridge          | 50    | 20  | 5   | Creates a bridge so that the player can cross a pit
| 3rd Eye         | 20    | 10  | 5   | Reveals Enemies on the minimap within its radius

##### Crafting?
| Part            | Cost  | HP  | Max Mana Input |  Description  |
| ----            | ----- | --- | ---               | ---           |
| Philosopher Stone | 50    | 30  | 5   | Converts trace into a resource over time
| Obliterator     | 50    | 30  | 5   | Converts resources into trace over time
| Cosmic Forge    | 100   | 30  | 5   | Allows player to enter the astral plane for a given item

##### Special?
| Part            | Cost  | HP  | Max Mana Input |  Description  |
| ----            | ----- | --- | ---               | ---           |
| Time Speed Up   | 50    | 10  | 5               | Speeds up time within the radius
| Time Speed Down | 50    | 10  | 5               | Slows time within the radius
| Familiar Nest   | 10    | 10  | 5               | Summons a bird the player can control for exploring the world
