/**
Notes:
gulp deploy --major
- build final packages of release install, trial install, source code
- should get version from gulpfile, increment by either major,minor or build (this is an arg like --major, --minor or --build (default))
- should upload to S3
- should deploy to local deploy
- should update website announcement
- should push documentation to docs website

gulp package
- build final packages of release install, trial install, source code
- should deploy to local test deploy

**/

var args = require('yargs').argv;
var runSequence = require('run-sequence');
var each = require('each-series');
var config = require('./gulp.config')();
var del = require('del');
var glob = require('glob');
var gulp = require('gulp');
var path = require('path');
var _ = require('lodash');
var $ = require('gulp-load-plugins')({lazy: true});
var colors = $.util.colors;
var envenv = $.util.env;
var BuildTarget;

// package
gulp.task("package", function() {
	each(config.targets, function(target, i, done) {
		log('Target ' + target.target);
		BuildTarget = target;
		runSequence('binaries', function() {
			done();
		});
	}, function(err) {
		console.log('Done!');
	});
});

gulp.task("resource", function() {
	log('Creating resource zip file');
    return gulp.src(config.resources)
        .pipe($.zip('resources.zip', {compress: false }))
		.pipe(gulp.dest(config.packagePath(BuildTarget.target)));
});

gulp.task("binaries", ['clean'], function(done) {
	runSequence('resource', 'other', 'build', function() {
		log('Copying binaries');
		return gulp.src(config.binSrc(BuildTarget.target))
			.pipe(gulp.dest(config.packagePath(BuildTarget.target)))
			.on('end',done);
	});
});

gulp.task("other", function() {
	log('Copying other');
    return gulp.src(config.other)
		.pipe(gulp.dest(config.packagePath(BuildTarget.target)));
});

gulp.task("build", function() {
	log('Building');
    return gulp.src(config.solutions)
        .pipe($.msbuild({
			configuration: 'Release',
			fileLoggerParameters: 'LogFile="' + path.join(config.logPath(), BuildTarget.target + '-Build.log') + '";Verbosity=diagnostic',
			logCommand: true,
			properties: { OutputPath: config.binPath(BuildTarget.target), DNNVersion: BuildTarget.target, TargetFrameworkVersion: BuildTarget.framework },
            targets: ['Clean', 'Build'],
            toolsVersion: 4.0,
			stdout: false
            })
        );
});

gulp.task('clean', function(done) {
var delconfig = [].concat(config.binPath(BuildTarget.target), config.packagePath(BuildTarget.target));
log('Cleaning: ' + $.util.colors.yellow(delconfig));
del(delconfig, done);
});


gulp.task("test", ['test1','test2','test3'], function(done) {
	log('Test starting...');
    return done();
});

gulp.task("test1", function(done) {
	log('Test1 starting...');
	pausecomp(3000);
	log('Test1 ending...');
return done();
});
gulp.task("test2", function() {
	log('Test2 starting...');
	pausecomp(3000);
	log('Test2 ending...');
return;
});
gulp.task("test3", function() {
	log('Test3 starting...');
	//pausecomp(1000);
	log('Test3 ending...');
return;
});

function pausecomp(ms) {
ms += new Date().getTime();
while (new Date() < ms){}
} 

// Default Task
gulp.task('default', ['test']);

/**
* Log a message or series of messages.
* Can pass in a string, object or array.
*/
function log(msg) {
	if (typeof(msg) === 'object') {
		for (var item in msg) {
			if (msg.hasOwnProperty(item)) {
				$.util.log($.util.colors.yellow(msg[item]));
			}
		}
	} else {
		$.util.log($.util.colors.yellow(msg));
	}
}