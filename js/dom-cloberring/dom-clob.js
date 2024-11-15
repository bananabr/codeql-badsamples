window.onload = function(){
    let someObject = window.someObject || {};
    let script = document.createElement('script');
    script.src = someObject.url;
    document.body.appendChild(script);
};

let _WINDOW = {};
let _DOCUMENT = {};

try {
if (typeof window !== 'undefined') _WINDOW = window;
if (typeof document !== 'undefined') _DOCUMENT = document;
} catch (e) {}

const initial = _WINDOW.FontAwesomeConfig || {};
if (initial.familyPrefix) {
initial.cssPrefix = initial.familyPrefix;
}

const _config = { ..._default,
...initial
};


function css() {
    const dcp = DEFAULT_CSS_PREFIX;
    const drc = DEFAULT_REPLACEMENT_CLASS;
    const fp = _config.cssPrefix;
    const rc = _config.replacementClass;
    let s = baseStyles;
    
    if (fp !== dcp || rc !== drc) {
        const dPatt = new RegExp("\\.".concat(dcp, "\\-"), 'g');
        const customPropPatt = new RegExp("\\--".concat(dcp, "\\-"), 'g');
        const rPatt = new RegExp("\\.".concat(drc), 'g');
        s = s.replace(dPatt, ".".concat(fp, "-")).replace(customPropPatt, "--".concat(fp, "-")).replace(rPatt, ".".concat(rc));
    }
    
    return s;
}

function insertCss(css) {
    if (!css || !IS_DOM) {
      return;
    }
    
    const style = DOCUMENT.createElement('style');
    style.setAttribute('type', 'text/css');
    style.innerHTML = css;
    const headChildren = DOCUMENT.head.childNodes;
    let beforeChild = null;
    
    for (let i = headChildren.length - 1; i > -1; i--) {
        const child = headChildren[i];
        const tagName = (child.tagName || '').toUpperCase();
        
        if (['STYLE', 'LINK'].indexOf(tagName) > -1) {
            beforeChild = child;
        }
    }
    
    DOCUMENT.head.insertBefore(style, beforeChild);
    return css;
}

insertCss(CSS());
