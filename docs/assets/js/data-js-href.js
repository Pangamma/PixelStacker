// ------------  [data-js-href]  ------------------------------------------------------------------
// On some context placement items we want a user to be able to click anywhere within a section to
// have it function like a hyperlink. That means ctrl + click or middle click should open link in
// new window, and regular click should open link in same window. Ensures no scrolling behavior
// starts in the event of a middle click. 

// If the user clicks a hyperlink directly, this behavior does nothing. The actual hyperlink takes
// higher priority, and none of this functionality should be used. This ensures you can have
// multiple links within a section.
// ------------------------------------------------------------------------------------------------

var dataJsHref = {
  target: null,
  origX: -1,
  origY: -1
};


// When user hovers over card, the anchor should get the underline effect
/* 
$(document).ready(function(){
	document.querySelectorAll('[data-js-href]').forEach(function(hrefElem) { 
		let attrHref = hrefElem.getAttribute('data-js-href');
		let matchingAnchors = hrefElem.querySelectorAll('[href="'+attrHref+'"]');
		matchingAnchors.forEach(function(anchorHref) {
			if (!anchorHref.classList.contains('f-href-target')){
				anchorHref.classList.add('f-href-target');
			}
		});
	});
});
*/

// When user middle clicks with mouse button, we want to prevent scroll behavior. Also,
// we want to track where they started their click so we can tell if they clicked or did
// some other operation on mouseup.
$(document).on('mousedown', '*[data-js-href]', function (e) {
  dataJsHref.origX = e.clientX;
  dataJsHref.origY = e.clientY;
  dataJsHref.target = e.target;
  if (e.button === 1) {
      if ($(e.target).closest('[data-js-href]').length > 0) {
          if ($(e.target).is(':not(button,a)')) {
              e.preventDefault();
              return false;
          }
      }
  }
});

// Enable middle click + click behavior for items within the data-js-href box.
$(document).on('mouseup', '*[data-js-href]', function (e) {
    if ($(e.target).is(':not(button,a)')) {
      if (e.target === dataJsHref.target) {
          if ((Math.max(e.clientX, dataJsHref.origX) - Math.min(e.clientX, dataJsHref.origX)) < 3) {
              if ((Math.max(e.clientY, dataJsHref.origY) - Math.min(e.clientY, dataJsHref.origY)) < 3) {
                  var elem = $(e.target).closest('[data-js-href]');
                  if (elem !== undefined) {
                      var isMiddleClick = e.button === 1 || e.ctrlKey === true;
                      var isRightClick = e.button === 2;
                      
                      var href = $(elem).attr('data-js-href');
                      if (isMiddleClick) {
                          window.open(href);
                          e.stopPropagation();
                          return false;
                      } else if (isRightClick) {
                          e.stopPropagation();
                          return false;
                      } else {
                          window.location = href;
                          e.stopPropagation();
                      }
                  }
              }
          }
      }
  }
});