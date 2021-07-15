var gTagAnalytics = document.createElement('script');
gTagAnalytics.src = 'https://www.google-analytics.com/analytics.js';
gTagAnalytics.async = true;

var gTagManager = document.createElement('script');
gTagManager.src = 'https://www.googletagmanager.com/gtm.js?id=GTM-NKMNNN';
gTagManager.async = true;

var noindex = document.createElement('meta');
noindex.name = 'robots';
noindex.content = 'noindex';

var head = document.getElementsByTagName('head')[0];
head.appendChild(noindex);
head.appendChild(gTagAnalytics);
head.appendChild(gTagManager);
