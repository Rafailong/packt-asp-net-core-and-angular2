(function (global) { 
  // map tells the System loader where to look for things 
  var map = { 
    'app': 'app', // our application files 
    '@angular': 'js/@angular', // angular2 packages 
    'rxjs': 'js/rxjs', // Rxjs package 
    'ngx-bootstrap': 'js/ngx-bootstrap'
  };

  // packages tells the system loader wich filename and/or extention
  // to look for by default (when none are specified)
  var packages = {
    'app': {
      main: 'main.js',
      defaultExtention: 'js'
    },
    'rxjs': {
      defaultExtention: 'js'
    },
    'ngx-bootstrap': {
      format: 'cjs', main: 'bundles/ngx-bootstrap.umd.js',
      defaultExtention: 'js'
    }
  };

  // configure @angular packages
  var ngPackageNames = [
    'common',
    'compiler',
    'core',
    'http',
    'platform-browser',
    'platform-browser-dynamic',
    'router',
    'forms'
  ];

  function packIndex(packageName) {
    packages['@angular/' + packageName] = {
      main: 'index.js',
      defaultExtention: 'js'
    };
  }

  function packUmd(packageName) {
    packages['@angular/' + packageName] = {
      main: '/bundles/' + packageName + '.umd.js',
      defaultExtention: 'js'
    };
  }

  var setPackageConfig = System.packageWithIndex ? packIndex : packUmd;
  ngPackageNames.forEach(setPackageConfig);
  var config = {
    'map': map,
    'packages': packages
  };

  System.config(config);
})(this);