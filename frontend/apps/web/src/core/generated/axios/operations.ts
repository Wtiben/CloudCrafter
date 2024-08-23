export const operations = {
    'PostCreateDeployment': {
        'path': '/api/Applications/:applicationId/deployment',
        'method': 'post'
    },
    'PostLoginUser': {
        'path': '/api/Auth/login',
        'method': 'post'
    },
    'PostCreateUser': {
        'path': '/api/Auth/create',
        'method': 'post'
    },
    'GetProjects': {
        'path': '/api/Projects',
        'method': 'get'
    },
    'CreateProject': {
        'path': '/api/Projects',
        'method': 'post'
    },
    'GetProject': {
        'path': '/api/Projects/:id',
        'method': 'get'
    },
    'post_api-projects-id': {
        'path': '/api/Projects/:id',
        'method': 'post'
    },
    'GetServers': {
        'path': '/api/Servers',
        'method': 'get'
    },
    'GetServerById': {
        'path': '/api/Servers/:id',
        'method': 'get'
    },
    'GetFilterableFields': {
        'path': '/api/System/get-fields',
        'method': 'get'
    },
    'GetTest': {
        'path': '/api/Test',
        'method': 'post'
    },
    'GetUsers': {
        'path': '/api/Users',
        'method': 'post'
    },
    'Test': {
        'path': '/api/Users/test',
        'method': 'get'
    }
} as const