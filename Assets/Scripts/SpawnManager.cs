using UnityEngine;
using System.Collections;


public class SpawnManager : MonoBehaviour {


/*
private void Start(){
//--Rect set up--
rect_message_nextWave = new Rect(Screen.width/2-80,
                                10,
                                160,
                                30);


WaveFinished (true);
}


//========Spawn Postions========
//Should be set from inspector
[System.Serializable]
public class SpawnPosition{
public Transform position; //drag and drop from inspector
public bool canBeUsed; //can this spawn be used, in case you to lock certain points
public float spawnDelay; //delay for each spawned enemy
//public short maxAmount; //max amount you can spawn at this point, so there would be no ugly grouping and over spawned enemies
};
public SpawnPosition[] spawnPositions;


//This will change spawn delay for certain spawn point
//It receives <spawnposition index, new delay>
private void SpawnDelay(short index, float newDelay){
//In case received index is not valid
if(index >= spawnPositions.Length || index < 0) Debug.Log("ERROR:[SpawnDelay] Received index "+index+" is not a valid index.");


//Received information can be set
else{
spawnPositions[index].spawnDelay = newDelay; //Set new delay for spawn position
}
}
//==============================




//========Enemy Types========
//Should be set from inspector
[System.Serializable]
public class EnemyType{
public GameObject enemyPrefab; //Drag your enemy prefab here
public int[] preferedSpawnPoints; //In case special type of enemy can (or should) spawn at specific locations, if size = 0 -> no prefered locations
//Numbers should be indexes of <spawnPositions> array you created before
public bool onStart; //Should this enemy be spawned on beggining of actual waves, if not you can set him/them manually using wave class from inspector 
public float ratio; //This is used only if <onStart> is false, for example, if ratio is 1.5 and amount of enemies this wave is 15, then this will be spawned 1 time.|.
};


public EnemyType[] enemyTypes;
//===========================




//========Caching actual Enemies========
//Inner structure, no need for inspector
//This is for keeping needed enemies to be spawned
private struct Enemy{
public int index;  //Index of the enemy type
public bool spawned; //when this enemy is spawned we toggle it as true, so we would not spawn this specific enemy again
};
private Enemy[] currentEnemies;  //All our currently spawned enemies


//Launched from
public void EnemyKilled(){
currentlyAlive--;
}
//======================================




//========Wave Logic========
//In case you want to unlock specific enemies only after certain wave
[System.Serializable]
public class Wave_Enemy{
public int index;
public int amount;
}


[System.Serializable]
public class Wave{
public int wave;  //wave we want to affect with these changes
public Wave_Enemy[] enemies; //enemies we want to add/spawn only this wave
public bool spawnOnlyThisWave; //Add for all continuing waves OR spwn just for this wave
};
public Wave[] waves;


//==========================




//========Actual Spawning Module========
private float spawnDelay = 0.4f; //spawn each enemy after certain time
private int startingAmount = 8; //start amount of emenies
private int addEachWave = 5; //how much we will add each wave


private int amountToSpawn = 0; //Current amount of enemies
private int spawnedAmount = 0; //How much we have spawned
private int currentlyAlive = 0;
private int leftAmount = 0;
private int MAX_ENEMY_AMOUNT = 8; //maximum amount of emenies in scene


private int currentWave = 1; //current wave
private const byte waitBeforeSpawn = 5; //How much wait before each wave spawning


//This is called when wave is finished
private void WaveFinished(bool firstLaunch){
if(!firstLaunch) currentWave ++; //add wave count


else{
if(MAX_ENEMY_AMOUNT < 20){
MAX_ENEMY_AMOUNT++;
}
}


spawnedAmount = 0;
currentlyAlive = 0;
amountToSpawn = (startingAmount + (addEachWave* (currentWave-1))); //add amount of enemies for next wave






leftAmount = EnemyAmountLeft(); //calculate how much enemies there will be next wave
SetSpawnEnemiesIndexes (); //Set current Enemies structure, what enemies to spawn and their order


Message_NextWave(); //activate GUI time count down before next wave
}


private void LaunchNextWave(){
InvokeRepeating("Spawner",0, spawnDelay); 
}


//Returns amount, how much emenies we still have to spawn
private int EnemyAmountLeft(){
return amountToSpawn - spawnedAmount;
}


//Returns true if all enemies are spawned in this wave, else, returns false
private bool GetAllEnemiesStatus(){
//check all current enemies
for(int i = 0; i < currentEnemies.Length; i++) {
if(! currentEnemies[i].spawned) return false;
}


return true;
}


//Create enemy type index array from enemy types indes - array of enemy numbers to be spawned
private void SetSpawnEnemiesIndexes(){
bool specialWaveEvent = false;
int specialWave = 0;
//check if there is special "WAVE" event
for(int iwave = 0; iwave < waves.Length; iwave++){
if(waves[iwave].wave == currentWave){
specialWaveEvent = true;
specialWave = iwave;
break;
}
}




//SPECIAL WAVE EVENT
if(specialWaveEvent){
int amountOfEnemies = 0;


//find out how much enemies there will be this round (from wave event)
for(int qq = 0; qq < waves[specialWave].enemies.Length; qq++){
amountOfEnemies += waves[specialWave].enemies[qq].amount;
}


//create array of next wave enemies
currentEnemies = new Enemy[amountOfEnemies];
//set everything to default
ResetCurrentEnemies();


//enemies lenght
for(int tt =0; tt < waves[specialWave].enemies.Length; tt++){
//amount of each enemy
for(int uu1 =0; uu1 <waves[specialWave].enemies[tt].amount; uu1++){
int thisEnemyIndex = waves[specialWave].enemies[tt].index;


int randomPos = Random.Range(0, currentEnemies.Length);
//maybe the saved current enemy is busy already, increment till we find empty spot
while(currentEnemies[randomPos].index != -1){


randomPos++;
if(randomPos > currentEnemies.Length-1) randomPos = 0;
}


//save enemy type index in array
currentEnemies[randomPos].index = thisEnemyIndex;
}
}


amountToSpawn = amountOfEnemies;
}


//ORIDINARY WAVE
else{
//create array of next wave enemies
currentEnemies = new Enemy[leftAmount];
//set everything to default
ResetCurrentEnemies();


//Loops through all enemy types, to get the ratio and calculate how much this wave they should be spawned
//We start from last enemy type because enemy with index 0 is the default enemy which all empty (-1) spots will be filled
for(int nn = enemyTypes.Length-1; nn >= 1; nn--){
//can this monster be used on start?
if(enemyTypes[nn].onStart){
float ratioAmount = leftAmount/10f;


//ratioAmount /= enemyTypes[nn].ratio;
//int spawnAmount = Mathf.FloorToInt(ratioAmount);
int spawnAmount = Mathf.FloorToInt( ratioAmount*enemyTypes[nn].ratio );


if(spawnAmount > 0){
//Sets actual enemy indexes in array
for(int tt =0; tt <spawnAmount; tt++){
int randomPos = Random.Range(0, currentEnemies.Length);
//maybe the saved current enemy is busy already, increment till we find empty spot
while(currentEnemies[randomPos].index != -1){
randomPos++;
if(randomPos > currentEnemies.Length-1) randomPos = 0;
}


//save enemy type index in array
currentEnemies[randomPos].index = nn;
}
}
}
}


//All "special enemy" slots are filled, now fill up the empty slots with default enemies
for(int w = 0; w < currentEnemies.Length; w++){
if(currentEnemies[w].index == -1){
currentEnemies[w].index = 0;
}
}
}
}


//Reset current enemies to default
private void ResetCurrentEnemies(){
for(int vv = 0; vv < currentEnemies.Length; vv++){
currentEnemies[vv].index = -1;
currentEnemies[vv].spawned = false;
}
}


//Spawns actual emenies
private void Spawner(){
//all enemies have not been spawned, yet
if(! GetAllEnemiesStatus()){
//check if scene already does not contain max enemies
if(currentlyAlive < MAX_ENEMY_AMOUNT){


for(int i =0; i <currentEnemies.Length; i++){
//This enemy has not been spawned yet
if(!currentEnemies[i].spawned){
int thisEnemyTypeIndex = currentEnemies[i].index;


//============Does this enemy type have preffered spawn point?=======
//Spawn point indexes which will be used for this enemy
int[] spawnPointIndexes;
int prefferedSpawnPointsIndexesSize = enemyTypes[ thisEnemyTypeIndex ].preferedSpawnPoints.Length;
//no preffered spawn points for this enemy
if(prefferedSpawnPointsIndexesSize == 0){
spawnPointIndexes = new int[spawnPositions.Length];
//assign positions
for(int yyy =0; yyy <spawnPositions.Length; yyy++){
spawnPointIndexes[yyy] = yyy;
}
}
//enemy does have preffered spawn points
else{
spawnPointIndexes = new int[ prefferedSpawnPointsIndexesSize ];


//assign positions for preffered spawn points
for(int yy =0; yy < prefferedSpawnPointsIndexesSize; yy++){
int prefIndex = enemyTypes[ thisEnemyTypeIndex ].preferedSpawnPoints[yy]; //get the index


spawnPointIndexes[yy] = prefIndex;
}
}
//===================================================================
//random spawn destination
int randomPos = Random.Range(0, spawnPointIndexes.Length);


for(int j =0; j <spawnPointIndexes.Length; j++){
int spawnArrIndex = spawnPointIndexes[randomPos];
//can this spawn be accessed
if(spawnPositions[spawnArrIndex].canBeUsed){
//is there a cool down on this spawn point
if(spawnPositions[spawnArrIndex].position.gameObject.GetComponent<Spawn>().Status()){
GameObject cachedEnemy = (GameObject)Instantiate(enemyTypes[ thisEnemyTypeIndex ].enemyPrefab, spawnPositions[spawnArrIndex].position.position, Quaternion.identity);
cachedEnemy.GetComponent<AI>().Set(this.gameObject);


//mark this enemy as spawned
currentEnemies[i].spawned = true;
currentlyAlive++;
spawnedAmount++;


//delay for spawn point
spawnPositions[spawnArrIndex].position.gameObject.GetComponent<Spawn>().Spawned( spawnPositions[spawnArrIndex].spawnDelay );


return;
}
}


//increment random spawn position
randomPos++;
if(randomPos >= spawnPointIndexes.Length-1) randomPos = 0; //be sure it does contain corretn index
}
}
}
}
}


else if(currentlyAlive <= 0){
CancelInvoke();
WaveFinished(false);
}
}
//======================================




//======================================================
private Rect rect_currentWave = new Rect(10,10, 120,20);
private Rect rect_enemiesLeft = new Rect(10,40, 120,20);
private Rect rect_message_nextWave;
private float amount_message_nextWave;
public GUIStyle style_numbers;


private bool message_nextWave = false;


private void Message_NextWave(){
amount_message_nextWave = waitBeforeSpawn;
message_nextWave = true;
}


private void OnGUI(){
GUI.Label (rect_currentWave, "Wave: "+currentWave, style_numbers); //renders currect wave
GUI.Label (rect_enemiesLeft, "Enemies left: "+EnemyAmountLeft().ToString(), style_numbers);


if(message_nextWave){
GUI.Label(rect_message_nextWave, "Wave starting in: "+amount_message_nextWave.ToString("F1"),style_numbers);


//Disable GUI mesage when time is out
if(amount_message_nextWave <= 0){
message_nextWave = false;
LaunchNextWave();
}
}
}


private void Update(){
if(message_nextWave){
amount_message_nextWave -= Time.deltaTime;
}
}
*/
}