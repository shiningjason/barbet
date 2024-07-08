module.exports = {
  extends: ['@0y0/eslint-config-react'],
  plugins: ['@babel/eslint-plugin'],
  parserOptions: {
    babelOptions: {
      plugins: [
        '@babel/plugin-syntax-jsx',
        ['@babel/plugin-proposal-decorators', { version: '2023-11' }],
        '@babel/plugin-transform-class-static-block',
        '@babel/plugin-proposal-class-properties'
      ]
    }
  },
  rules: {
    'no-undef': 'off',
    '@babel/no-undef': 'error'
  }
}
