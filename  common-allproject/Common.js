//Repl all
function removeClass_uctFrmSelItem() {
	var element = document.getElementById('Content_uctFrmSelItem1_btnOK');
	var classname = "aspNetDisabled";
	var cn = element.className;
	var rxp = new RegExp("\\s?\\b" + classname + "\\b", "g");
	cn = cn.replace(rxp, '');
	element.className = cn;

}