// moment.js language configuration
// language : hungarian (hu)
// author : Adam Brunner : https://github.com/adambrunner
(function(){function a(a,b,c,d){var e=a;switch(c){case"s":return d||b?"n\u00e9h\u00e1ny m\u00e1sodperc":"n\u00e9h\u00e1ny m\u00e1sodperce";case"m":e="egy";case"mm":return e+(d||b?" perc":" perce");case"h":e="egy";case"hh":return e+(d||b?" \u00f3ra":" \u00f3r\u00e1ja");case"d":e="egy";case"dd":return e+(d||b?" nap":" napja");case"M":e="egy";case"MM":return e+(d||b?" h\u00f3nap":" h\u00f3napja");case"y":e="egy";case"yy":return e+(d||b?" \u00e9v":" \u00e9ve");default:}return""}function b(a){var b="";switch(this.day()){case 0:b="vas\u00e1rnap";break;case 1:b="h\u00e9tf\u0151n";break;case 2:b="kedden";break;case 3:b="szerd\u00e1n";break;case 4:b="cs\u00fct\u00f6rt\u00f6k\u00f6n";break;case 5:b="p\u00e9nteken";break;case 6:b="szombaton"}return(a?"":"m\u00falt ")+"["+b+"] LT[-kor]"}var c={months:"janu\u00e1r_febru\u00e1r_m\u00e1rcius_\u00e1prilis_m\u00e1jus_j\u00fanius_j\u00falius_augusztus_szeptember_okt\u00f3ber_november_december".split("_"),monthsShort:"jan_feb_m\u00e1rc_\u00e1pr_m\u00e1j_j\u00fan_j\u00fal_aug_szept_okt_nov_dec".split("_"),weekdays:"vas\u00e1rnap_h\u00e9tf\u0151_kedd_szerda_cs\u00fct\u00f6rt\u00f6k_p\u00e9ntek_szombat".split("_"),weekdaysShort:"v_h_k_sze_cs_p_szo".split("_"),longDateFormat:{LT:"H:mm",L:"YYYY.MM.DD.",LL:"YYYY. MMMM D.",LLL:"YYYY. MMMM D., LT",LLLL:"YYYY. MMMM D., dddd LT"},calendar:{sameDay:"[ma] LT[-kor]",nextDay:"[holnap] LT[-kor]",nextWeek:function(){return b.call(this,!0)},lastDay:"[tegnap] LT[-kor]",lastWeek:function(){return b.call(this,!1)},sameElse:"L"},relativeTime:{future:"%s m\u00falva",past:"%s",s:a,m:a,mm:a,h:a,hh:a,d:a,dd:a,M:a,MM:a,y:a,yy:a},ordinal:function(a){return"."}};typeof module!="undefined"&&module.exports&&(module.exports=c),typeof window!="undefined"&&this.moment&&this.moment.lang&&this.moment.lang("hu",c)})();