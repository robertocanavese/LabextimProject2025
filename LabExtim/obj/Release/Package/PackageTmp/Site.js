function OpenSelector(page) {
    window.open(page, 'ItemPicker', 'scrollbars=2,resizable=2,width=800,height=750,left=120,top=120');
}

function OpenItem(page) {
    newWindow = window.open(page, 'Item', 'width=425,height=500,left=120,top=120,dependant=yes,directories=no,location=no,menubar=no,status=no,titlebar=no,toolbar=no,scrollbars=2,resizable=yes');
    if (window.focus) {
        newWindow.focus()
    }
}

function OpenBigItem(page) {
    newWindow = window.open(page, 'Item_Details', 'width=1250,height=600,left=120,top=120,dependant=yes,directories=no,location=no,menubar=no,status=no,titlebar=no,toolbar=no,scrollbars=2,resizable=yes');
    if (window.focus) {
        newWindow.focus()
    }
}

function OpenBigItemNarrow(page) {
    newWindow = window.open(page, 'Item_Details', 'width=700,height=1000,left=50,top=10,dependant=yes,directories=no,location=no,menubar=no,status=no,titlebar=no,toolbar=no,scrollbars=2,resizable=yes');
    if (window.focus) {
        newWindow.focus()
    }
}

// 20141119 DAVIDE BOGNOLO
// AGGIUNTO PARAMENTRO ALLA CHIAMATA OpenBigItem2 in modo da personalizzare la finestra di apertura per poidkey e abilitare in questo modo la possibilità
// di avere più finestre aperte di diversi ordini.
function OpenBigItem2(page, poIdKey) {
    newWindow = window.open(page, 'Item_Details' + poIdKey, 'width=1250,height=700,left=120,top=120,dependant=yes,directories=no,location=no,menubar=no,status=no,titlebar=no,toolbar=no,scrollbars=2,resizable=yes');
    if (window.focus) {
        newWindow.focus()
    }
}

function OpenBigItem3(page) {
    newWindow = window.open(page, 'Note_Details', 'width=1250,height=700,left=120,top=120,dependant=yes,directories=no,location=no,menubar=no,status=no,titlebar=no,toolbar=no,scrollbars=2,resizable=yes');
    if (window.focus) {
        newWindow.focus()
    }
}

function OpenReportTextsItem(page) {
    newWindow = window.open(page, 'Item', 'width=500,height=750,left=120,top=120,dependant=yes,directories=no,location=no,menubar=no,status=no,titlebar=no,toolbar=no,scrollbars=2,resizable=yes');
    if (window.focus) {
        newWindow.focus()
    }
}
//var newwindow;
//function poptastic(url) {
//    newwindow = window.open(url, 'name', 'height=400,width=200');
//    if (window.focus) { newwindow.focus() }
//}