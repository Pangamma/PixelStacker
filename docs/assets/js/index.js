function printImageCard($imgURL, $header, $text, $cta, $href, $dataGrid) {
    let html = '\
    <div data-grid="{{dataGrid}}">\
        <div class="c-content-placement image-card" data-js-href="{{href}}">\
            <picture class="hide-on-mobile">\
                <img src="{{imgURL}}" alt="" role="presentation">\
            </picture>\
            <div>\
                <h2 class="c-subheading-4 hide-on-mobile">{{header}}</h2>\
                <h2 class="c-heading-4 show-on-mobile">{{header}}</h2>';

    if (!!$text && $text.trim().length > 0) {
        html += '<p class="c-paragraph-3" style="padding-top:0px;">{{text}}</p>';
    }

    if (!!$cta && $cta.trim().length > 0) {
        html += '\
                <p class="c-paragraph-3" style="padding-top:0px;">\
                    <a href="{{href}}" class="c-hyperlink f-href-target" aria-label="{{cta}} {{header}}">\
                        {{cta}}\
                    </a>\
                </p>';
    }
    html += '</ div>\
            <div class="show-on-mobile">\
                <br />\
                <picture>\
                    <img src="{{imgURL}}" alt="" role="presentation" style="position:relative;width:100%;">\
                </picture>\
            </div>\
        </div>\
    </div>\
</div>';

    html = html
		.replace(/{{dataGrid}}/g,$dataGrid || 'col-4')
        .replace(/{{imgURL}}/g, $imgURL)
        .replace(/{{href}}/g, $href)
        .replace(/{{cta}}/g, $cta)
        .replace(/{{header}}/g, $header)
        .replace(/{{text}}/g, $text);

    document.write(html);
}