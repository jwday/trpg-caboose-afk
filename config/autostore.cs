function startSlots() {
	echo("");
	echo(">> Auto-storing valuable items.");
	echo("");
	$Slots = true;
	slotLoop(); 
}

function slotLoop() {
	if(getItemCount("Keldrinite") > 0) {
		sell("Keldrinite"); }

	if(getItemCount("Diamond") > 0) {
		sell("Diamond");
        $coin::total = $coin::total + 2652; }

	if(getItemCount("Emerald") > 0) {
		sell("Emerald");
        $coin::total = $coin::total + 1552; }

	if(getItemCount("Titanium") > 0) {
		sell("Titanium");
        $coin::total = $coin::total + 1332; }

	if(getItemCount("Silver") > 0) {
		sell("Silver");
        $coin::total = $coin::total + 664; }

	if(getItemCount("Sapphire") > 0) {
		sell("Sapphire");
        $coin::total = $coin::total + 468; }

	if(getItemCount("Topaz") > 0) {
		sell("Topaz");
        $coin::total = $coin::total + 256; }

	if(getItemCount("Iron") > 0) {
		sell("Iron");
        $coin::total = $coin::total + 232; }

	if(getItemCount("Ruby") > 0) {
		sell("Ruby"); }

	if(getItemCount("Nickel") > 0) {
		sell("Nickel"); }

	if(getItemCount("Gold") > 0) {
		sell("Gold"); }

	if(getItemCount("Dragon Scale") > 0) {
		sell("Dragon scale"); }

	if(getItemCount("Claymore") > 2) {
		sell("Claymore");
        $coin::total = $coin::total + 27484; }

	if(getItemCount("Gladius") > 2) {
		sell("Gladius"); }

	if(getItemCount("Bastard Sword") > 2) {
		sell("Bastard Sword"); }

	if(getItemCount("Enchanted Stone") > 0) {
		sell("Enchanted Stone"); }

	if(getItemCount("Crystal Blue Potion") > 5) {
		sell("Crystal Blue Potion"); }
	
    if($Slots) {
	schedule("slotLoop();", 1);  }
}


function savecharLoop() {
	say(0, "#savecharacter");
	if($Slots) {
		schedule("savecharLoop();", 30);
	}
}

function stopSlots() {
	$Slots = false;
	echo("");
	echo(">> No longer auto-storing items.");
	echo("");
}




//To Start Storing items, type startSlots();
//To End Storing items, type endSlots();
//To Start Save loop, type savecharLoop();