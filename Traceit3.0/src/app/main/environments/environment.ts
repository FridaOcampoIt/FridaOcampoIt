export const environment_dev = {
    name: 'Traceit (DEV)',
    production: false,
    hmr: false,
    apiUrl: 'http://localhost:8000',
    hashidSecret: 'local_pass',
	cryptoSecret: 'local_pass',
};

export const environment_prod = {
    name: 'Traceit',
    production: true,
    hmr: false,
    apiUrl: '',
    hashidSecret: 'local_pass',
	cryptoSecret: 'local_pass',
};

export const environment_hmr = {
    name: 'Traceit (HMR)',
    production: false,
    hmr: true,
    apiUrl: 'http://localhost:8000',
    hashidSecret: 'local_pass',
	cryptoSecret: 'local_pass',
};

