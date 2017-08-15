(function(){
 
// Change this list to the valid characters you want
var validChars = "\(\)\$0123456789\.,";
 
// Init the regex just once for speed - it is "closure locked"
var
    //str = jQuery.fn.dataTableExt.oApi._fnEscapeRegex( validChars ),
    re = new RegExp('[^'+validChars+']');
 
 
jQuery.fn.dataTableExt.aTypes.unshift(
   function ( data )
    {
        if ( typeof data !== 'string' || re.test(data) ) {
            return null;
        }
 
        return 'currency';
    }
);
 
}());

jQuery.extend( jQuery.fn.dataTableExt.oSort, {
	"currency-pre": function (a) {
		var hasParen = a.indexOf(')') > 0;
    	a = (a === "-") ? 0 : a.replace(/[^\d\-\.]/g, "");
    	//a = a.replace(")", "").replace("(", "-");
        return parseFloat( hasParen ? -a : a );
    },
 
    "currency-asc": function ( a, b ) {
        return a - b;
    },
 
    "currency-desc": function ( a, b ) {
        return b - a;
    }
} );