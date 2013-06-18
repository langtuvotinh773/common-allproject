//Repl all
function removeClass_uctFrmSelItem() {
	var element = document.getElementById('Content_uctFrmSelItem1_btnOK');
	var classname = "aspNetDisabled";
	var cn = element.className;
	var rxp = new RegExp("\\s?\\b" + classname + "\\b", "g");
	cn = cn.replace(rxp, '');
	element.className = cn;

}
// Chieu cao tu dong thay doi
 function ChangeHeightPopup() {
            var height = $('#wrapper-table').height()-20;
            $('.child-flist').each(function () {
                $(this).css('height', height + 'px');
                if ($(this).css('position') == 'fixed')
                    $(this).css({ 'top': 0, 'left': $('.wrapper-right').offset().left + 'px', 'width': '725px' });
              
            });