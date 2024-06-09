using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class DungeonGeneration : MonoBehaviour
{
    public static DungeonGeneration Instance { get; private set; }

    public RoomTemplate[] templates = new RoomTemplate[10];
    public HashSet<Vector3Int> used = new HashSet<Vector3Int>();
    public HashSet<Room> rooms = new HashSet<Room>();
    public Room last;

    public Tilemap tileMap;

    public Tile doorTL, doorTR, doorBL, doorBR;
    public Tile[,] doorTiles;

    //           0         1          2         3        4         5     6     7   8       9         10       11   12          13      14
    public Tile ground, trCorner, tlCorner, brCorner, blCorner, bottom, top, left, right, gLeft, gtlCorner, gTop, gtrCorner, gRight;//doorTR
    public Tile[] tiles;

    public Tile chestA, chestB, chestC;

    public GameObject chestTrigger;
    public Dictionary<Vector3Int, Tile> chestTypes = new Dictionary<Vector3Int, Tile>();

    public bool p = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tiles = new Tile[] { ground, trCorner, tlCorner, brCorner, blCorner, bottom, top, left, right, gLeft, gtlCorner, gTop, gtrCorner, gRight, doorTR };
        doorTiles = new Tile[,]{ {doorTR,ground,ground,gTop,doorBR},
                      {doorTL,ground,ground,gTop,doorBL},
                      {doorTR,gLeft,ground,gRight,doorTL},
                      {doorBR,gLeft,ground,gRight,doorBL} };

        generateTemplates();

        Room r = new Room(templates[0], -5, -5);
        rooms.Add(r);
        r.display();
        r.register();

        r.treasure = true;
        r.addChest(DungeonGeneration.Instance.chestB);
        r.run(20, 0, true);

        EnemyGeneration.Instance.generateEnemies();
    }

    void Update()
    {

    }

    public void generateTemplates()
    {
        int[,] basic1x1 = { {2,6,6,6,6,6,6,6,1},
                            {7,10,11,11,11,11,11,12,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {4,5,5,5,5,5,5,5,3} };

        bool[,] basic1x1Doors = { { true }, { true }, { true }, { true } };

        int[,] basic2x1 = { {2,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1},
                            {7,10,11,11,11,11,11,11,11,11,11,11,11,11,11,11,12,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,3} };
        bool[,] basic2x1Doors = { { true, false }, { true, false }, { true, true }, { true, true } };

        int[,] basic1x2 = { {2,6,6,6,6,6,6,6,1},
                            {7,10,11,11,11,11,11,12,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,13,8},
                            {4,5,5,5,5,5,5,5,3} };
        bool[,] basic1x2Doors = { { true, true }, { true, true }, { true, false }, { true, false } };

        int[,] basic2x2 = { {2,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1},
                            {7,10,11,11,11,11,11,11,11,11,11,11,11,11,11,11,12,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,3} };
        bool[,] basic2x2Doors = { { true, true }, { true, true }, { true, true }, { true, true } };

        int[,] l2x2 = {     {2,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1},
                            {7,10,11,11,11,11,11,11,11,11,11,11,11,11,11,11,12,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {7,9,0,0,0,0,0,0,0,0,0,0,0,0,0,0,13,8},
                            {4,5,5,5,5,5,5,5,5,14,0,0,0,0,0,0,13,8},
                            {-1,-1,-1,-1,-1,-1,-1,-1,-1,7,0,0,0,0,0,0,13,8},
                            {-1,-1,-1,-1,-1,-1,-1,-1,-1,7,0,0,0,0,0,0,13,8},
                            {-1,-1,-1,-1,-1,-1,-1,-1,-1,7,0,0,0,0,0,0,13,8},
                            {-1,-1,-1,-1,-1,-1,-1,-1,-1,7,0,0,0,0,0,0,13,8},
                            {-1,-1,-1,-1,-1,-1,-1,-1,-1,7,0,0,0,0,0,0,13,8},
                            {-1,-1,-1,-1,-1,-1,-1,-1,-1,7,0,0,0,0,0,0,13,8},
                            {-1,-1,-1,-1,-1,-1,-1,-1,-1,7,0,0,0,0,0,0,13,8},
                            {-1,-1,-1,-1,-1,-1,-1,-1,-1,7,0,0,0,0,0,0,13,8},
                            {-1,-1,-1,-1,-1,-1,-1,-1,-1,4,5,5,5,5,5,5,5,3} };
        bool[,] l2x2Doors = { { false, true }, { true, true }, { false, true }, { true, true } };

        templates[0] = new RoomTemplate(9, 9, basic1x1, basic1x1Doors, "basic1x1");
        templates[1] = new RoomTemplate(18, 9, basic2x1, basic2x1Doors, "basic2x1");
        templates[2] = new RoomTemplate(9, 18, basic1x2, basic1x2Doors, "basic1x2");
        templates[3] = new RoomTemplate(18, 18, basic2x2, basic2x2Doors, "basic2x2");
        templates[4] = new RoomTemplate(18, 18, l2x2, l2x2Doors, "l2x2");
    }

    public class RoomTemplate
    {
        public int width, height;
        public int[,] pixels;
        public bool[,] doors;
        public string type;

        public RoomTemplate(int w, int h, int[,] p, bool[,] d, string type)
        {
            this.width = w;
            this.height = h;

            this.pixels = new int[p.GetLength(1), p.GetLength(0)];
            for (int i = 0; i < p.GetLength(0); i++)
            {
                for (int j = 0; j < p.GetLength(1); j++)
                {
                    this.pixels[j, p.GetLength(0) - i - 1] = p[i, j];
                }
            }

            this.doors = d;
            this.type = type;
        }

        public int getDoors(int dir)
        {
            if (dir == 0 || dir == 1) return height / 9;
            if (dir == 2 || dir == 3) return width / 9;
            return -1;
        }
    }

    public class Room
    {
        public RoomTemplate template;
        public int x, y;

        public int dir, lastDoor, door, danger;
        public bool treasure;

        //left, right, down, up
        public int[] dirX = { -9, 9, 0, 0 };
        public int[] dirY = { 0, 0, -9, 9 };

        public Room(RoomTemplate template, int x, int y)
        {
            this.template = template;

            this.x = x;
            this.y = y;
        }

        public Room(RoomTemplate template, int x, int y, int dir, int lastDoor, int door)
        {
            this.template = template;

            this.x = x;
            this.y = y;

            this.dir = dir;
            this.lastDoor = lastDoor;
            this.door = door;
        }

        public void run(int depth, int danger, bool main)
        {
            if (depth <= 0)
            {
                if (main)
                {
                    this.treasure = true;
                    EnemyGeneration.Instance.generate(x + 13, y + 15, EnemyGeneration.Instance.boss);
                    DungeonGeneration.Instance.last = this;
                }

                return;
            }

            runNext(depth - 1, danger + 1, main);
            if (Random.Range(0, 100) < 20) runNext(4, danger + 1, false);
        }

        public void runNext(int depth, int danger, bool main)
        {
            Room next = generate();
            if (next == null) return;

            if (depth == 0)
            {
                if (!main)
                {
                    next.treasure = true;
                    next.addChest(DungeonGeneration.Instance.chestB);
                    next.generateTreasure();
                }
            }
            else next.generateChests();

            next.danger = danger;
            DungeonGeneration.Instance.rooms.Add(next);

            if (main && depth == 0 && !next.template.type.Equals("basic2x2")) depth++;
            next.run(depth, danger, main);
        }

        public void generateTreasure()
        {
            addChest(DungeonGeneration.Instance.chestB);

            if (Random.Range(1, 100) < 50)
            {
                generateTreasure();
            }
        }

        public void generateChests()
        {
            if (Random.Range(1, 100) < 20)
            {
                addChest(DungeonGeneration.Instance.chestA);
                generateChests();
            }
        }

        public Room generate()
        {
            ArrayList pos = new ArrayList();

            //loop through possible room templates
            foreach (RoomTemplate t in DungeonGeneration.Instance.templates)
            {
                if (t == null) continue;

                //loop through all possible directions
                for (int dir = 0; dir < 4; dir++)
                {
                    //if (dir != 3) continue;

                    int oDir = getOpposite(dir);

                    //loop through all doors from current room
                    for (int curDoor = 0; curDoor < template.getDoors(dir); curDoor++)
                    {
                        if (!template.doors[dir, curDoor]) continue; //skip bad doors

                        //loop through all doors from next room
                        for (int nextDoor = 0; nextDoor < t.getDoors(oDir); nextDoor++)
                        {
                            if (!t.doors[getOpposite(dir), nextDoor]) continue; //skip bad doors

                            int xLoc = x;
                            if (dir == 0) xLoc += dirX[dir] * t.width / 9;
                            if (dir == 1) xLoc += dirX[dir] * template.width / 9;

                            int yLoc = y;
                            if (dir == 2) yLoc += dirY[dir] * t.height / 9;
                            if (dir == 3) yLoc += dirY[dir] * template.height / 9;

                            if (dir == 0 || dir == 1) yLoc += 9 * (curDoor - nextDoor);
                            else xLoc += 9 * (curDoor - nextDoor);

                            bool good = true;

                            //check if tiles are open
                            for (int i = 0; i < t.width; i += 9)
                            {
                                for (int j = 0; j < t.height; j += 9)
                                {
                                    Vector3Int loc = new Vector3Int(xLoc + i, yLoc + j);
                                    if (DungeonGeneration.Instance.used.Contains(loc)) good = false;
                                }
                            }

                            if (good) pos.Add(new Room(t, xLoc, yLoc, dir, curDoor, nextDoor));
                        }
                    }
                }
            }

            if (pos.Count == 0) return null;

            int rand = Random.Range(0, pos.Count);

            Room r = (Room)(pos[rand]);
            int rDir = r.dir;
            int rLastDoor = r.lastDoor;
            int rDoor = r.door;

            //connect the rooms
            drawDoor(rDir, rLastDoor);

            r.display();
            r.drawDoor(getOpposite(rDir), rDoor);
            r.register();

            return (Room)pos[rand];
        }

        public void addChest(Tile c)
        {
            int count = 0;

            for (int i = 0; i < template.width; i++)
            {
                for (int j = 0; j < template.height; j++)
                {
                    if (template.pixels[i, j] == 0) count++;
                }
            }

            int rand = Random.Range(0, count);

            count = 0;
            for (int i = 0; i < template.width; i++)
            {
                for (int j = 0; j < template.height; j++)
                {
                    if (template.pixels[i, j] == 0)
                    {
                        if (count == rand)
                        {
                            createHitbox(x + i, y + j, c);
                            drawTile(c, x + i, y + j);
                            return;
                        }
                        else count++;
                    }
                }
            }
        }

        public void createHitbox(int x, int y, Tile c)
        {
            GameObject o = Instantiate(DungeonGeneration.Instance.chestTrigger);

            Vector3Int vec = new Vector3Int(x + 3, y + 2);
            o.transform.position = vec;
            if (!DungeonGeneration.Instance.chestTypes.ContainsKey(vec)) DungeonGeneration.Instance.chestTypes.Add(vec, c);
        }

        public void drawDoor(int dir, int dNum)
        {
            int sx = x;
            int sy = y;

            if (dir == 1) sx += template.width - 1;
            if (dir == 3) sy += template.height - 1;

            if (dir == 0 || dir == 1)
            {
                sy += dNum * 9 + 2;

                for (int i = 0; i < 5; i++)
                {
                    drawTile(DungeonGeneration.Instance.doorTiles[dir, i], sx, sy + i);
                    if (i != 0 && i != 4) drawTile(DungeonGeneration.Instance.ground, sx + (dir == 0 ? 1 : -1), sy + i);
                }
            }
            if (dir == 2 || dir == 3)
            {
                sx += dNum * 9 + 2;

                for (int i = 0; i < 5; i++)
                {
                    drawTile(DungeonGeneration.Instance.doorTiles[dir, i], sx + i, sy);
                    if (i != 0 && i != 4) drawTile(DungeonGeneration.Instance.ground, sx + i, sy + (dir == 2 ? 1 : -1));
                }
            }
        }

        public int getOpposite(int dir)
        {
            if (dir == 0) return 1;
            if (dir == 1) return 0;

            if (dir == 2) return 3;
            if (dir == 3) return 2;

            return -1;
        }

        public void register()
        {
            for (int i = 0; i < template.width; i += 9)
            {
                for (int j = 0; j < template.height; j += 9)
                {
                    DungeonGeneration.Instance.used.Add(new Vector3Int(x + i, y + j));
                }
            }
        }

        public void display()
        {
            for (int i = 0; i < template.width; i++)
            {
                for (int j = 0; j < template.height; j++)
                {
                    int cur = template.pixels[i, j];

                    if (cur == -1) continue;
                    drawTile(DungeonGeneration.Instance.tiles[cur], x + i, y + j);
                }
            }
        }

        public void drawTile(Tile t, int x, int y)
        {
            DungeonGeneration.Instance.tileMap.SetTile(new Vector3Int(x, y), t);
        }
    }
}
