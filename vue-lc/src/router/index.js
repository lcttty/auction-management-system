import Vue from 'vue'
import Router from 'vue-router'
import Login from '@/components/Login'
import Register from '@/components/Register'
import Home from '@/components/Home'
import AppIndex from '@/components/AppIndex'
import PaiMai from '@/components/PaiMai'
import Special from '@/components/Special'
import Buy from '@/components/Buy'
import Myself from '@/components/Myself'
import News from '@/components/News'
import Shangjia from '@/components/Shangjia'
import Houtai from '@/components/Houtai'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'

Vue.use(Router)
Vue.use(ElementUI)
const routerPush = Router.prototype.push
Router.prototype.push = function push (location) {
  return routerPush.call(this, location).catch(error => error)
}
const router = new Router({
  routes: [
    {
      path: '/',
      redirect: '/index'
    },
    {
      path: '/home',
      name: 'Home',
      component: Home,
      redirect: '/index',
      children: [
        {
          path: '/index',
          name: 'AppIndex',
          component: AppIndex,
          meta: {
            charset: 'utf-8',
            title: '首页'
          }
        },
        {
          path: '/paimai/:query',
          name: 'PaiMai',
          component: PaiMai,
          meta: {
            charset: 'utf-8',
            title: '拍卖'
          }
        },
        {
          path: '/special',
          name: 'Special',
          component: Special,
          meta: {
            charset: 'utf-8',
            title: '专场'
          }
        },
        {
          path: '/buy',
          name: 'Buy',
          component: Buy,
          meta: {
            charset: 'utf-8',
            title: '购买页面'
          }
        },
        {
          path: '/myself',
          name: 'Myself',
          component: Myself,
          meta: {
            charset: 'utf-8',
            title: '个人控制台'
          }
        },
        {
          path: '/news',
          name: 'News',
          component: News,
          meta: ['charset', 'utf-8']
        }
      ]
    },
    {
      path: '/login',
      name: 'Login',
      component: Login,
      meta: {
        charset: 'utf-8',
        title: '登录'
      }
    },
    {
      path: '/register',
      name: 'Register',
      component: Register,
      meta: {
        charset: 'utf-8',
        title: '注册'
      }
    },
    {
      path: '/shangjia',
      name: 'Shangjia',
      component: Shangjia,
      meta: {
        charset: 'utf-8',
        title: '商家控制台'
      }
    },
    {
      path: '/houtai',
      name: 'Houtai',
      component: Houtai,
      meta: {
        charset: 'utf-8',
        title: '管理员控制台'
      }
    }
  ]
})

router.beforeEach((to, from, next) => {
  if (to.meta.title) {
    document.title = to.meta.title
  }
  next()
})

export default router
