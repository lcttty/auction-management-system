// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import { JSEncrypt } from 'jsencrypt'

Vue.config.productionTip = false
Vue.prototype.$getCode = function (password) {
  let encrypt = new JSEncrypt()
  encrypt.setPublicKey('MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCbp2AQD2uHohhX/GWAnUzacJm2GzFMGQZ6rl5Fy90ktxLB6xy7ADucqbicLPw2rbEM19JWnIfC25I1Y6PGyDT2nWi+wCtvRvnoz/QjurRZO3iiMgUV1c63DsIPJF6+CX5CwcUAgribk5y2Ro//hT096iGv8OydCuhq7K42JiMT/QIDAQAB')
  let data = encrypt.encrypt(password)
  return data
}
var axios = require('axios')
axios.defaults.baseURL = 'http://localhost:55587/api'
axios.defaults.withCredentials = true
Vue.prototype.indexOf = function (val) {
  for (var i = 0; i < this.length; i++) {
    if (this[i] === val) return i
  }
  return -1
}
Vue.prototype.remove = function (val) {
  var index = this.indexOf(val)
  if (index > -1) {
    this.splice(index, 1)
  }
}
Vue.prototype.$axios = axios
/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  components: { App },
  template: '<App/>'
})
