
function ontableinput(e) {
    if (e.target instanceof HTMLTableCellElement) {
        console.log(e);
    }
}
async function ontabledblclick(ev) {
    let table = ev.currentTarget;
    let nodeName = ev.target.nodeName;
    if (nodeName == "TD") {
        let dotnetref = table.dotnetref;
        if (!dotnetref) return;
        let cell = ev.target;
        if (!cell) return;
        let rowId = parseInt(cell.dataset.rowidx, 10);
        let colid = parseInt(cell.dataset.colidx, 10);
        if (rowId==null || !colid==null) return;
        await dotnetref.invokeMethodAsync('OnTableCellDblClick', rowId, colid);
    }
}
async function ontablefocusout(ev) {
    let table = ev.currentTarget;
    let parentNode = ev.target.parentNode;
    if (!parentNode && parentNode.nodeName != "TD") return;

    let dotnetref = table.dotnetref;
    if (!dotnetref) return;
    await dotnetref.invokeMethodAsync('OnTableCellDblClick', null, null);

}

// Options for the observer (which mutations to observe)
const observerConfig = { attributes: true, childList: true, subtree: true };

// Callback function to execute when mutations are observed
//const observerCallback = (mutationList, observer) => {
//    for (const mutation of mutationList) {
//        if (mutation.type === "childList") {
//            console.log("A child node has been added or removed.");
//        } else if (mutation.type === "attributes") {
//            //console.log(`The ${mutation.attributeName} attribute was modified.`);
//        } else if (mutation.type === "subtree") {
//            console.log(`subtree was modified.`);
//        }
//    }
//};
// Create a MutationObserver instance
const observerCallback = (mutations) => {
    let tdAdded = false;
    let table = null;
    mutations.forEach((mutation) => {
        // Check for added nodes
        if (mutation.addedNodes.length > 0) {
            mutation.addedNodes.forEach((node) => {
                if (!tdAdded && node.tagName == "TD" && !table) {
                    tdAdded = true;
                    table = node.parentNode.parentNode.parentNode;
                }
                // Check if the added node is an input or contains inputs
                if (node.nodeType === 1) { // Element node
                    // Check if the node itself is an input in a td
                    if (node.tagName === 'INPUT' && node.closest('td')) {
                        //console.log('Input added to td:', node);
                        node.focus();
                    }
                    else {
                        // Check if the node contains any inputs in tds
                        const input = node.querySelector('td input');
                        if (input) input.focus();
                    }
                }
            });
        }
    });
    if (tdAdded && table) {
        if (!table.dataset.isColumnSizeFixed) {
            table.dataset.isColumnSizeFixed = true;
            var ths = table.querySelectorAll("thead>tr:first-child>th");
            let cols = table.querySelectorAll("col");
            let totalWidth = 0;
            for (let i = 0; i < ths.length; i++)
            {
                let col = cols[i];
                col.style.width = ths[i].clientWidth + 'px';
                totalWidth += ths[i].clientWidth;
            }
            table.style.width = totalWidth + 'px';
        }
    }
};
const tableObserver = new MutationObserver(observerCallback);

export async function initTable(dotnetref, id) {
    let table = document.getElementById(id);
    if (!table) {
        console.error("table not found");
        return;
    }
    //let cells = table.querySelectorAll("td");
    //if (!cells || cells.length == 0) return;
    //cells.forEach(cell => {
    //    cell.contentEditable = true;
    //});
    table.dotnetref = dotnetref;
    table.addEventListener("dblclick", ontabledblclick);
    table.addEventListener("focusout", ontablefocusout);
    table.addEventListener("input", ontableinput);

    tableObserver.observe(table, observerConfig);
}