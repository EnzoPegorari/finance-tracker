<script setup lang="ts">
import { Button } from '~/components/ui/button'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '~/components/ui/card'
import { Input } from '~/components/ui/input'
import { Label } from '~/components/ui/label'

definePageMeta({ layout: 'auth' })

const { register } = useAuth()
const router = useRouter()

const name = ref('')
const email = ref('')
const password = ref('')
const isSubmitting = ref(false)
const errorMessage = ref('')

async function onSubmit() {
  errorMessage.value = ''
  isSubmitting.value = true
  try {
    await register(name.value, email.value, password.value)
    await router.push('/dashboard')
  }
  catch (error: any) {
    errorMessage.value = error?.data?.statusMessage || error?.data?.error || 'Não foi possível criar a conta.'
  }
  finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <Card class="w-full max-w-sm">
    <CardHeader>
      <CardTitle class="text-2xl">
        Criar conta
      </CardTitle>
      <CardDescription>Comece a organizar suas finanças</CardDescription>
    </CardHeader>
    <CardContent>
      <form class="flex flex-col gap-4" @submit.prevent="onSubmit">
        <div class="flex flex-col gap-1.5">
          <Label for="name">Nome</Label>
          <Input id="name" v-model="name" type="text" required autocomplete="name" />
        </div>
        <div class="flex flex-col gap-1.5">
          <Label for="email">E-mail</Label>
          <Input id="email" v-model="email" type="email" placeholder="voce@exemplo.com" required autocomplete="email" />
        </div>
        <div class="flex flex-col gap-1.5">
          <Label for="password">Senha</Label>
          <Input id="password" v-model="password" type="password" minlength="8" required autocomplete="new-password" />
        </div>
        <p v-if="errorMessage" class="text-sm text-destructive">
          {{ errorMessage }}
        </p>
        <Button type="submit" class="w-full" :disabled="isSubmitting">
          {{ isSubmitting ? 'Criando conta...' : 'Criar conta' }}
        </Button>
      </form>
      <p class="mt-4 text-center text-sm text-muted-foreground">
        Já tem uma conta?
        <NuxtLink to="/login" class="text-primary underline-offset-4 hover:underline">
          Entrar
        </NuxtLink>
      </p>
    </CardContent>
  </Card>
</template>
